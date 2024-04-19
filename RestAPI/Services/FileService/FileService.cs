using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Data;
using RestAPI.Models;

namespace RestAPI.Services.FileService
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly AppDbContext _dbContext;

        public FileService(IWebHostEnvironment webHost, AppDbContext dbContext)
        {
            _webHost = webHost;
            _dbContext = dbContext;
        }



        public async Task<IActionResult> UploadImage(Image image)
        {
            try
            {
                // Check if the image file is provided
                if (image.ImageFile == null || image.ImageFile.Length <= 0)
                {
                    return new BadRequestObjectResult("Image file is required.");
                }

                // Generate a unique file name for the uploaded image
                string uniqueFileName = await SaveImageToServer(image);

                // Create a new Image entity with the uploaded image details
                var newImage = new Image
                {
                    Title = image.Title,
                    ImagePath = uniqueFileName
                };

                // Add the new Image entity to the DbContext and save changes to the database
                await _dbContext.Images.AddAsync(newImage);
                await _dbContext.SaveChangesAsync();

                // Return a success response
                return new OkObjectResult("Image uploaded successfully.");
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        private async Task<string> SaveImageToServer(Image image)
        {
            string uniqueFileName = null;

            // Generate a unique file name
            uniqueFileName = Guid.NewGuid().ToString() + "_" + image.ImageFile.FileName;

            // Define the upload path within the wwwroot directory
            string uploadFolder = Path.Combine(_webHost.WebRootPath, "Uploads/Images");

            // Ensure the upload directory exists; create if not
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            // Combine the upload folder path with the unique file name
            string filePath = Path.Combine(uploadFolder, uniqueFileName);

            // Copy the uploaded image to the specified file path
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.ImageFile.CopyToAsync(fileStream);
            }

            // Return the unique file name to be stored in the database
            return uniqueFileName;
        }

   
    }
}
