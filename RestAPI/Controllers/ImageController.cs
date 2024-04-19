using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Models;
using RestAPI.Services.FileService;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IFileService _fileService;

        public ImageController(IFileService fileService)
        {
            _fileService = fileService;
        }


        [HttpGet("Get")]
        public IActionResult Get()
        {
            var images = _fileService.getImages();

            return Ok(images);

        }

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadImage( IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            try
            {
                string imagePath = await _fileService.UploadFile(file);
                return Ok(new { ImagePath = imagePath });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error uploading file: {ex.Message}");
            }
        }
    }
}
