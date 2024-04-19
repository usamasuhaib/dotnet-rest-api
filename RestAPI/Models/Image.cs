using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestAPI.Models
{
    public class Image
    {
        [Key]
        public string Id { get; set; }



        [Required]
        public string Title { get; set; }

        public string ImagePath { get; set; }

        [NotMapped]

        public IFormFile ImageFile { get; set; }
    }
}
