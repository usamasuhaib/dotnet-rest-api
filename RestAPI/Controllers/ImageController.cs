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


        [HttpPost]
        public async Task<IActionResult> UploadImage([FromBody] Image img)
        {
            if (ModelState.IsValid)
            {
                await _fileService.UploadImage(img);
            }

            return BadRequest("Invalid image data.");
        }
    }
}
