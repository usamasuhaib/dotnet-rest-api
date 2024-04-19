using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestAPI.Models
{
    public class AppUser : IdentityUser
    {
        public string ImagePath  { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }


    }
}
