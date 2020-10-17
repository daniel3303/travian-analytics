using System.ComponentModel.DataAnnotations;

namespace TravianAnalytics.Dtos.Auth {
    public class LoginDto {
        [Required(ErrorMessage = "O username é obrigatório.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "A password é obrigatória.")]
        public string Password { get; set; }
    }
}
