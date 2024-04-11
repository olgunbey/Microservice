using FreeCourse.IdentityServer.Models;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using static IdentityServer4.IdentityServerConstants;

namespace FreeCourse.IdentityServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(LocalApi.PolicyName)]
    public class UserController : ControllerBase
    {
        private UserManager<AppUser> _appUser;
        public UserController(UserManager<AppUser> appUser)
        {
            _appUser = appUser;
        }
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody]SignUpDto signUpDto)
        {
          var result= await _appUser.CreateAsync(new AppUser()
            {
                UserName = signUpDto.UserName,
                City = signUpDto.City,
                Email = signUpDto.Email,
            }, signUpDto.Password);


            if(!result.Succeeded)
            {
                return BadRequest(ResponseDto<NoContent>.Fail(result.Errors.Select(y=>y.Description).ToList(),400));
            }
            return NoContent();
        }
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userIdClaim= User.Claims.FirstOrDefault(y => y.Type == JwtRegisteredClaimNames.Sub);
               if (userIdClaim == null)
                   return BadRequest();
               var user =await _appUser.FindByIdAsync(userIdClaim.Value);

               if(user == null)
                   return BadRequest();

               return Ok(user);
        }
        
    }
}
