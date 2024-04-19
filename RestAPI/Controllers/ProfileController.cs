using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models;
using System.Security.Claims;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfileController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("Users")]
        public async Task<IActionResult> getUsersList()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
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
            };

            return Ok(profileData);
        }
    }
}
