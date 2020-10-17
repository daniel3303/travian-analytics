using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravianAnalytics.Controllers.Abstract;
using TravianAnalytics.Data;
using TravianAnalytics.Dtos;
using TravianAnalytics.Models;
using TravianAnalytics.Models.Player;

namespace TravianAnalytics.Controllers {

    public class ApiController : BaseController {
        public ApiController(ApplicationDbContext dbContext) : base(dbContext) { }


        [HttpPost]
        [AllowAnonymous]
        [EnableCors("Travian")]
        public async Task<IActionResult> Report(ReportDto reportDto) {
            if (ModelState.IsValid) {
                var alliance =
                    await _dbContext.Alliances.FirstOrDefaultAsync(a => a.TravianId == reportDto.Alliance.TravianId);
                if (alliance == null) {
                    alliance = new Alliance() {
                        Name = reportDto.Alliance.Name,
                        TravianId = reportDto.Alliance.TravianId
                    };
                    _dbContext.Add(alliance);
                }

                var reportTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                reportTime = reportTime.AddMinutes(-reportTime.Minute % 30);
                
                if (alliance.LastUpdate >= reportTime) return Ok("Report already received for this time period.");
                alliance.LastUpdate = reportTime;
                
                foreach (var playerDto in reportDto.Players) {
                    var player = alliance.Players.FirstOrDefault(p => p.TravianId == playerDto.TravianId);
                    if (player == null) {
                        player = new Player() {
                            Alliance = alliance,
                            Name = playerDto.Name,
                            TravianId = playerDto.TravianId
                        };
                        alliance.Players.Add(player);
                        _dbContext.Add(new Notification() {
                            Message = $"O jogador {player.Name} juntou-se à aliança {player.Alliance.Name}."
                        });
                    } else {
                        player.Name = playerDto.Name;
                    }

                    var previousRecord = player.Records.OrderByDescending(r => r.Time).FirstOrDefault();
                    var record = new Record() {
                        Online = playerDto.Online,
                        Player = player,
                        Population = playerDto.Population,
                        Villages = playerDto.Villages,
                        Time = reportTime
                    };
                    player.Records.Add(record);
                    
                    // Notification of new or lost villages
                    if (previousRecord != null) {
                        if (previousRecord.Villages > record.Villages) {
                            _dbContext.Add(new Notification() {
                               Message = $"O jogador {player.Name} perdeu uma aldeia."
                            });
                        } else if(previousRecord.Villages < record.Villages) {
                            _dbContext.Add(new Notification() {
                                Message = $"O jogador {player.Name} fundou/conquistou uma aldeia."
                            });
                        }
                    }
                }

                var playersWhoLeftTheAlly = alliance.Players.Where(p => reportDto.Players.All(dto => dto.TravianId != p.TravianId));
                foreach (var player in playersWhoLeftTheAlly) {
                    _dbContext.Add(new Notification() {
                        Message = $"O jogador {player.Name} abandonou a aliança {player.Alliance.Name}."
                    });
                    player.Alliance = null;
                }

                await _dbContext.SaveChangesAsync();
                return Ok(true);
            }

            return Ok(false);
        }

    }
}