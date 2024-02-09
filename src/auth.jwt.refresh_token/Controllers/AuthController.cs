using auth.jwt.refresh_token.Dtos.Auth;
using auth.jwt.refresh_token.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace auth.jwt.refresh_token.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(ILogger<AuthController> logger, JwtTokenService jwtService) : ControllerBase
    {

        private readonly JwtTokenService _jwtService = jwtService;

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginRequestDTO request)
        {
            // Validate the credential against the user record queried from the database

            // Mimicking Login Success
            bool IsLoginSuccess = Convert.ToBoolean(Random.Shared.Next(0, 2));


            // Early return pattern
            if (IsLoginSuccess)
            {
                logger.LogWarning("{Email} tried to log in with incorrect credential!", request.Email);
                return Unauthorized("Account with given UserName or password not found!");
            }


            // JWT payload storing user information as key-value pair using CLaim
            var claims = new List<Claim>
                {
                    new (JwtRegisteredClaimNames.Sub, Convert.ToString(Random.Shared.Next(1_00_000,9_99_999))),
                    new (JwtRegisteredClaimNames.GivenName, request.Email.Split("@")[0]),
                    new (JwtRegisteredClaimNames.Email, request.Email)
                };

            var jwtToken = _jwtService.GenerateJwtToken(claims);

            return Ok(jwtToken);            
        }
    }
}
