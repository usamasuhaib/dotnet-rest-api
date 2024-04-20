
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models;

namespace RestAPI.Services.FileService
{
    public interface IFileService
    {
        public Task<string> UploadFile(IFormFile file);

        public IEnumerable<Image> getImages();

        public Task DeleteImage(int id);


    }
}
