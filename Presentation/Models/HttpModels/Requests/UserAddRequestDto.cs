using CosmeticShop.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace HttpModels.Requests
{
    public class UserAddRequestDto
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
        public string Email { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Пароль не должен быть меньше 8 символов!", MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        public RoleType Role { get; set; }
    }
}