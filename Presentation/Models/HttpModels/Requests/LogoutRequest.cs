using System.ComponentModel.DataAnnotations;

namespace HttpModels.Requests
{
    public class LogoutRequest
    {
        [Required]
        public string Token { get; set; }
    }
}