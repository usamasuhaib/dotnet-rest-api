using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.DTOs.Requests
{
    public class UserLoginRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
