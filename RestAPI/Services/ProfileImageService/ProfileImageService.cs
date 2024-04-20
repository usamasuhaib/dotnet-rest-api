using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models;
using System.IO;

namespace RestAPI.Services.ProfileImageService
{
    public class ProfileImageService : IProfileImageService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _webHost;

        public ProfileImageService(UserManager<AppUser> userManager, IWebHostEnvironment webHost)
        {
            _userManager = userManager;
            _webHost = webHost;
        }


        public async Task UploadProfile(IFormFile file, string? id)
        {

            var appUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id==id);

            string uniqueFileName = string.Empty;

            if (appUser.ImagePath != null)
            {
                string filePath = Path.Combine(_webHost.ContentRootPath, "Uploads", appUser.ImagePath);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                };
            }
         
             uniqueFileName = UploadImage(file, id);

             appUser.ImagePath = uniqueFileName;

             await _userManager.UpdateAsync(appUser);


        }




        //private string UploadImage(IFormFile file, string id)
        //{
        //    List<string> ValidExtensions = new List<string>()
        //      { ".jpg", ".png", ".jpeg",".gif" };

        //    string extension = Path.GetExtension(file.FileName);

        //    //size

        //    long size = file.Length;
        //    if (size > (5 * 1024 * 1024))
        //    {
        //        throw new NotImplementedException("Maximum size can be 5mb");
        //    }


        //    //namechanging

        //    string fileName = Guid.NewGuid().ToString() + extension;
        //    string path = Path.Combine(_webHost.ContentRootPath, "Uploads/Images");

        //    using FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
        //    file.CopyTo(stream);

        //    return fileName;
        //}








        private string UploadImage(IFormFile file, string userId)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentNullException(nameof(file), "File is empty or invalid.");
            }

            string uploadFolderPath = Path.Combine(_webHost.ContentRootPath, "Uploads/Images");
            if (!Directory.Exists(uploadFolderPath))
            {
                Directory.CreateDirectory(uploadFolderPath);
            }

            string fileName = $"{userId}_{Path.GetFileName(file.FileName)}";
            string filePath = Path.Combine(uploadFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return fileName; // Return the unique file name (or full file path) for the uploaded image
        }











        public Task DeleteProfileImage(string id)
        {
            throw new NotImplementedException();
        }
    }
}
