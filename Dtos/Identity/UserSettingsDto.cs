using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TravianAnalytics.Models.Identity.Abstract;

namespace TravianAnalytics.Dtos.Identity {
    [AutoMap(typeof(User))]
    public class UserSettingsDto {

        [Required(ErrorMessage = "A posição das mensagens é obrigatória")]
        public string MessagesPosition { get; set; }
    }
}