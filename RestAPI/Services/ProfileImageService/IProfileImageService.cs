using Microsoft.AspNetCore.Mvc;

namespace RestAPI.Services.ProfileImageService
{
    public interface IProfileImageService
    {



        public Task UploadProfile(IFormFile file, string? id);


        public Task DeleteProfileImage(string id);
    }
}
