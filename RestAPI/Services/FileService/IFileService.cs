
using Microsoft.AspNetCore.Mvc;
using RestAPI.Models;

namespace RestAPI.Services.FileService
{
    public interface IFileService
    {


        Task<IActionResult> UploadImage(Image image);

    }
}
