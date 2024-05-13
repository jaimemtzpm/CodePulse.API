using CodePulse.API.Models.Dto;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager,
                              ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        // POST: {apibaseurl}/api/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request){
            var identityUser = await this.userManager.FindByEmailAsync(request.Email);
            if (identityUser is not null) {
                var checkPasswordResult = await this.userManager.CheckPasswordAsync(identityUser, request.Password);

                if (checkPasswordResult) {
                    var roles = await this.userManager.GetRolesAsync(identityUser);
                    // Create a Token and Response
                    var jwtToken = this.tokenRepository.CreateJwtToken(identityUser, roles.ToList());
                    var response = new LoginResponseDto {
                        Email = request.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken
                    };

                    return Ok(response);
                } 
            }     

            ModelState.AddModelError("", "Email or Password Incorrect");

            return ValidationProblem(ModelState);
        }

        // POST: {apibaseurl}/api/auth/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request){
            // Create IdentityUser object
            var user = new IdentityUser {
                UserName = request.Email?.Trim(),
                Email = request.Email?.Trim()
            };
            var identityResult = await userManager.CreateAsync(user, request.Password);

            if (identityResult.Succeeded) {
                // Add Role to user
                identityResult = await userManager.AddToRoleAsync(user, "Reader");
                if (identityResult.Succeeded){
                    return Ok();
                }
            } 

            if (identityResult.Errors.Any()){
                foreach (var error in identityResult.Errors){
                    ModelState.AddModelError("", error.Description);
                }
            }

            return ValidationProblem(ModelState);
        }        
     
    
    }
}