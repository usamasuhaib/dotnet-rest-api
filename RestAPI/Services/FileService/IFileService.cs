
using Microsoft.AspNetCore.Mvc;
using RestAPI.Models;

namespace RestAPI.Services.FileService
{
    public interface IFileService
    {

        public Task<string> UploadFile(IFormFile file);

        public IEnumerable<Image> getImages();


    }
}
