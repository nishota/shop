using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IOptions<JwtSettings> _jwtSettings;
        private readonly AuthInfoService _authInfoService;

        public AuthenticationController(
             ILogger<AuthenticationController> logger,
             IOptions<JwtSettings> jwtSettings,
             AuthInfoService authInfoService)
        {
            _logger = logger;
            _jwtSettings = jwtSettings;
            _authInfoService = authInfoService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(AuthInfo authInfo)
        {
            if (authInfo != null && authInfo.Email != null && authInfo.Password != null)
            {
                var user = await GetUser(authInfo.Email, authInfo.Password);

                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                        // TODO: これなに? optionalなのでとりあえず抜く
                        // new Claim(JwtRegisteredClaimNames.Sub, _jwtSettings.Value.Subject), 
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(nameof(user.Email), user.Email),
                        // new Claim(nameof(user.Password), user.Password) // TODO: password入れないっぽい?
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Key));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        // TODO: これなに? optionalなのでとりあえずnullにする
                        _jwtSettings.Value.Issuer,
                        // TODO: これなに? optionalなのでとりあえずnullにする
                        _jwtSettings.Value.Audience, 
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<AuthInfo?> GetUser(string email, string password)
        {
            // TODO: passwordの暗号化。
            // TODO: いったん平文でDBに入れる。
            return await _authInfoService.GetAsync(email, password);
        }
    }
}
