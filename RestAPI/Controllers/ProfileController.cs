using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models;
using RestAPI.Services.ProfileImageService;
using System.Security.Claims;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IProfileImageService _profileImageService;

        public ProfileController(UserManager<AppUser> userManager, IProfileImageService profileImageService)
        {
            _userManager = userManager;
            _profileImageService = profileImageService;
        }

        [HttpGet("Users")]
        public async Task<IActionResult> getUsersList()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }


        [HttpPatch("upload")]
        public async Task<IActionResult> UploadProfileImage(IFormFile file, string? id)
        {
            try
            {
                await _profileImageService.UploadProfile(file, id);
                return Ok("Profile image uploaded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error uploading profile image: {ex.Message}");
            }
        }





        [HttpGet("Details")]
        [Authorize]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> getProfileData()
        {


            // Get the user's email
            var userEmail = User.FindFirstValue(ClaimTypes.Email);


            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                return NotFound(new { ErrorMessage = "User not found" });
            }

            // Return user profile data
            var profileData = new
            {
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                ImagePath=user.ImagePath
            };

            return Ok(profileData);
        }
    }
}
