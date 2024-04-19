using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Data;
using RestAPI.Models;
using static System.Net.Mime.MediaTypeNames;

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

        public IEnumerable<Models.Image> getImages()
        {
            return _dbContext.Images.ToList();
        }
       
    
        public async Task<string> UploadFile(IFormFile file)
        {

            //extension
            List<string> ValidExtensions = new List<string>()
            {
                ".jpg",
                ".png",
                ".jpeg",
                ".gif",
            };

            string extension=Path.GetExtension(file.FileName);

            //size

            long size = file.Length;
            if (size > (5 * 1024*1024))
            {
                throw new NotImplementedException("Maximum size can be 5mb");
            }

            //namechanging

            string fileName=Guid.NewGuid().ToString()+ extension;
            //string path = Path.Combine(Directory.GetCurrentDirectory(),"Uploads/images");
            string path = Path.Combine(_webHost.ContentRootPath, "Uploads/Images");


            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using FileStream stream = new FileStream(Path.Combine(path,fileName), FileMode.Create);
            file.CopyTo(stream);




            //upload to server


            var newImage = new Models.Image()
            {  
                ImagePath = fileName,
            };
            await _dbContext.Images.AddAsync(newImage);
            await _dbContext.SaveChangesAsync();


            return fileName;

        }


        public async Task DeleteImage(int id)
        {
            var img = _dbContext.Images.FirstOrDefault(x => x.Id == id);

            //var img = _dbContext.Images.Find(id);
            if (img == null)
            {
                throw new InvalidOperationException($"Image with ID {id} not found.");
            }
            else
            {

                //string path = Path.Combine(_webHost.ContentRootPath, "Uploads/Images");

                string deleteFromFolder = Path.Combine(_webHost.ContentRootPath, "Uploads/Images");
                string currentImage = Path.Combine(Directory.GetCurrentDirectory(), deleteFromFolder, img.ImagePath);
                if (currentImage != null)
                {
                    if (System.IO.File.Exists(currentImage))
                    {
                        System.IO.File.Delete(currentImage);
                    };
                };

                _dbContext.Images.Remove(img);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
