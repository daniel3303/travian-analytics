using System.ComponentModel.DataAnnotations;

namespace TravianAnalytics.Dtos.Identity {
    public class SetPasswordDto {

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "A nova password é obrigatória.")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "A confirmação password é obrigatória.")]
        [Compare(nameof(NewPassword), ErrorMessage = "As passwords não são iguais")]
        public string ConfirmPassword { get; set; }

    }
}