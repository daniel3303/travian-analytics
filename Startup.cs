using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NonFactors.Mvc.Grid;
using TravianAnalytics.Data;
using TravianAnalytics.Helpers;
using TravianAnalytics.Infrastructure;
using TravianAnalytics.Models.Identity.Abstract;
using TravianAnalytics.Services;
using TravianAnalytics.Services.Components.CronJobs;
using TravianAnalytics.Services.Components.Events;
using TravianAnalytics.Services.Components.FlashMessage;
using TravianAnalytics.Services.Contracts.CronJobs;
using TravianAnalytics.Services.Contracts.Events;
using TravianAnalytics.Services.Contracts.FlashMessage;

namespace TravianAnalytics {
    public class Startup {
        private IConfiguration Configuration { get; }
        private readonly bool _isProduction;

        public Startup(IConfiguration configuration, IHostEnvironment env) {
            Configuration = configuration;
            _isProduction = env.IsProduction();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddSingleton(Configuration);
            var cookieTime = TimeSpan.FromMinutes(Configuration.GetSection("Configurations").GetValue<int>("LoginTime"));
            var cookieSecure = Configuration.GetSection("Configurations").GetValue<bool>("RedirectHttps")
                ? CookieSecurePolicy.Always
                : CookieSecurePolicy.SameAsRequest;

            // Register the EF application database context using proxies for lazy loading
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies()
            );
            
            // Token expiration (ie. recover password token, confirm email token...)
            services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(24));

            // Identity
            services.AddAuthentication();
            
            // Rewrite urls as lowercase
            services.AddRouting(options => options.LowercaseUrls = true);

            // Service for configurations
            services.AddTransient<ConfigurationsService>();

            services.AddHttpClient();
            services.AddHttpContextAccessor();

            services.AddScoped<RedirectService>();
            
            services.AddAutoMapper(typeof(Startup));

            // Adds flash session messages (ie. success, error, info messages to the user)
            services.AddTransient<IFlashMessage, FlashMessage>();
            services.AddTransient<IFlashMessageSerializer, JsonFlashMessageSerializer>();

            // Lifecycle events
            services.AddScoped<EntityChangeTracker>();
            
            // Cors
            services.AddCors(o => o.AddPolicy("Travian", builder => {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            // Adds Identity
            services.AddDefaultIdentity<User>(options => {
                options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;
            }).AddRoles<Role>() // Uses the Role class for authorization
              .AddEntityFrameworkStores<ApplicationDbContext>() // Tells Identity to fetch the users from this database
              .AddClaimsPrincipalFactory<AwareClaimsPrincipalFactory>();
            
            
            // Services for DDD events
            // https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-events-design-implementation
            // Register the event dispatcher
            services.AddTransient<IEventDispatcher, EventDispatcher>();
            var serviceHandlers =
                typeof(Startup).Assembly.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(IEventHandler)));

            // Registers all the event handlers
            foreach (var serviceHandler in serviceHandlers) {
                services.Add(new ServiceDescriptor(typeof(IEventHandler), serviceHandler, ServiceLifetime.Transient));
            }
            
            // Cron jobs related services
            services.AddTransient<ICronJobManager, CronJobManager>();
            var cronJobSubscribers = typeof(Startup).Assembly.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(ICronJobSubscriber)));

            // Registers all the cron job subscribers
            foreach (var cronJobSubscriber in cronJobSubscribers) {
                services.Add(new ServiceDescriptor(typeof(ICronJobSubscriber), cronJobSubscriber, ServiceLifetime.Transient));
            }

            // Adds session for storage
            services.AddDistributedMemoryCache();
            services.AddSession(options => {
                options.Cookie.Name = "SessionCookie";
                options.Cookie.SecurePolicy = cookieSecure;
                options.IdleTimeout = cookieTime;

                //options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Adds controllers
            if (_isProduction) {
                services.AddControllersWithViews().AddNewtonsoftJson(options => {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
            } else {
                services.AddControllersWithViews().AddNewtonsoftJson(options => {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                }).AddRazorRuntimeCompilation();
            }


            // Anti forgery settings
            services.AddAntiforgery(options => {
                options.FormFieldName = "AntiForgery";
                options.HeaderName = "X-CSRF-TOKEN";
                options.SuppressXFrameOptionsHeader = false;
                options.Cookie.Name = "AntiForgeryCookie";
                options.Cookie.Expiration = cookieTime;
                options.Cookie.SecurePolicy = cookieSecure;
            });

            // Cookie for temp data
            services.Configure<CookieTempDataProviderOptions>(options => {
                options.Cookie.Name = "TempDataCookie";
                options.Cookie.Expiration = cookieTime;
                options.Cookie.SecurePolicy = cookieSecure;
            });

            // Configures the Identity by cookie
            services.ConfigureApplicationCookie(options => {
                options.ExpireTimeSpan = cookieTime;
                options.Cookie.Name = "AuthCookie";
                options.Cookie.SecurePolicy = cookieSecure;
                options.Events = new CookieAuthenticationEvents {
                    OnRedirectToLogin = ctx => {
                        var requestPath = ctx.Request.Path;
                        ctx.Response.Redirect("/auth/login");
                        return Task.CompletedTask;
                    },
                    OnRedirectToAccessDenied = ctx => {
                        var requestPath = ctx.Request.Path;
                        ctx.Response.Redirect("/auth/accessdenied");
                        return Task.CompletedTask;
                    },
                };
            });

            // Password security
            services.Configure<IdentityOptions>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 1;
            });


            // Create an access policy for each claim
            services.AddAuthorization(options => {
                // Creates one policy for each claim
                foreach (var claimGroup in ClaimStore.ClaimGroups()) {
                    foreach (var claim in claimGroup.Claims) {
                        options.AddPolicy(claim.Type, policy => {
                            policy.RequireClaim(claim.Type); //Enforce the policy
                        });
                    }
                }
            });

            //MVC Grid
            services.AddMvcGrid(filters => {
                filters.BooleanFalseOptionText = () => "Não";
                filters.BooleanTrueOptionText = () => "Sim";
                filters.BooleanEmptyOptionText = () => "";
            });

            services.AddMvc(options => {
                options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => "O valor inserido é inválido.");
                options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor((x) => "O valor inserido é inválido.");
                options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(x => "Campo obrigatório.");
                options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => "Campo obrigatório.");
                options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => "Campo obrigatório.");
                options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => "O valor inserido é inválido.");
                options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor((x) => "O valor inserido é inválido.");
                options.ModelBindingMessageProvider.SetValueIsInvalidAccessor((x) => "O valor inserido é inválido.");
                options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "O valor inserido tem de ser numérico.");
                options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => "O valor inserido deve ser numérico.");
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => "O valor não pode ser vazio.");
            }).AddDataAnnotationsLocalization().AddViewLocalization();

            // Routes
            services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env, ConfigurationsService configurations) {
            // Use static files
            app.UseStaticFiles(new StaticFileOptions { OnPrepareResponse = ctx => { ctx.Context.Response.Headers.Append("Cache-Control", "public, max-age=604800"); }});

            // Uses routes
            app.UseRouting();
            
            // Cors
            app.UseCors("Travian");

            // Uses authentication
            app.UseAuthentication();

            // Use authorization
            app.UseAuthorization();

            // Uses session
            app.UseSession();
            
            // Session handler
            app.UseMiddleware<SessionHandler>();



            if (_isProduction) {
                // Show errors in page
                app.UseHsts();

                // Error Handling
                app.UseStatusCodePages();

                // Redirect options (www and https)
                var rewriteOptions = new RewriteOptions();
                if (configurations.RedirectWww) {
                    rewriteOptions.AddRedirectToWwwPermanent();
                }

                if (configurations.RedirectHttps) {
                    app.UseHttpsRedirection();
                    rewriteOptions.AddRedirectToHttpsPermanent();
                }

                app.UseRewriter(rewriteOptions);
            } else {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<RedirectHandler>();

            // Map routes
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
