using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RestAPI.Configuration;
using RestAPI.Models;
using RestAPI.Models.DTOs.Requests;
using RestAPI.Models.DTOs.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public AuthenticationController(UserManager<AppUser> userManager, IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        //[HttpGet("Users")]
        //public async Task<IActionResult> getUsersList()
        //{
        //    var users = await _userManager.Users.ToListAsync();
        //    return Ok(users);


        //}


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto model)
        {
            if (ModelState.IsValid)
            {
                var userExists= await _userManager.FindByEmailAsync(model.Email);
                if(userExists == null)
                {
                    var newUser = new AppUser
                    {
                        UserName=model.UserName,
                        Email=model.Email
                    };
                    var isCreated= await _userManager.CreateAsync(newUser, model.Password);
                    if(isCreated.Succeeded)
                    {
                        var jwtToken= GenerateJwtToken(newUser);
                        return Ok(new RegistrationResponse
                        {
                            Success = true,
                            Token=jwtToken,
                     
                        });
                    }
                    return BadRequest(new RegistrationResponse
                    {
                        Success = false,
                        Errors = new List<string>
                        {
                            "Server Error Please try agian later"
                        }
                    });

                };
                return BadRequest(new RegistrationResponse
                {
                    Success = false,
                    Errors = new List<string>
                    {
                        "Email already in User , please try with another email"
                    }
                });
            }

            return BadRequest(new RegistrationResponse
            {
                Success = false,
                Errors= new List<string>
                {
                    "Invalid Payload , please try again"
                }
            });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto user)
        {
            if (ModelState.IsValid)
            {
                var userExist= await _userManager.FindByEmailAsync(user.Email);
                if (userExist != null)
                {
                    var passwordMatched= await _userManager.CheckPasswordAsync(userExist,user.Password);
                    if (passwordMatched)
                    {
                        var jwtToken= GenerateJwtToken(userExist);
                        return Ok(new LoginResponse
                        {
                            Success = true,
                            Token = jwtToken,

                        });

                    }

                    return BadRequest(new LoginResponse
                    {
                        Success = false,
                        Errors = new List<string>
                        {
                            "Incorrect Password, please Enter Correct Password"
                        }
                    });

                }

                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Errors = new List<string>
                {
                    "Email not found , please try with another email"
                }
                });

            }


            return BadRequest(new LoginResponse
            {
                Success = false,
                Errors = new List<string>
                {
                    "Invalid Payload , please try again"
                }
            });
        }



        //method to generate jwt token
        private string GenerateJwtToken(AppUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);

            var TokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),

                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
                }),
                Expires = DateTime.Now.AddHours(2),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
            };

            var token = jwtTokenHandler.CreateToken(TokenDescriptor);

            var jwtToken= jwtTokenHandler.WriteToken(token);
            return jwtToken;
        }



        [HttpGet("Profile")]
        [Authorize]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task< IActionResult> getProfileData()
        {

            // Get the user's ID
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get the user's email
            var userEmail = User.FindFirstValue(ClaimTypes.Email);


            // Use the userId to fetch the user's profile data from the database
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
                // Add other user profile properties as needed
            };

            return Ok(profileData);
        }



    }
}
