using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.DTOs.Requests
{
    public class UserRegistrationRequestDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email  { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
