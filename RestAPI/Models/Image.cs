using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestAPI.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }


        public string ImagePath { get; set; }

        [NotMapped]

        public IFormFile ImageFile { get; set; }
    }
}
