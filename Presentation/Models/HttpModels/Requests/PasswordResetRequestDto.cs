using System.ComponentModel.DataAnnotations;

namespace HttpModels.Requests
{
    public class PasswordResetRequestDto
    {
        [Required]
        [StringLength(30, ErrorMessage = "Пароль не должен быть меньше 8 символов!", MinimumLength = 8)]
        public string NewPassword { get; set; }
    }
}