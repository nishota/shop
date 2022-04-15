using api.Models;
using api.Models.Settings;
using api.Models.Schemas;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using api.Services.Interface;

namespace api.Controllers
{
    [ApiController]
    [Route("api/auth/[controller]")]
    public class SignInController : ControllerBase
    {
        // TODO: Log出力
        // private readonly ILogger<AuthenticationController> _logger;
        private readonly IOptions<JwtSettings> _jwtSettings;
        private readonly AuthInfoStore _authInfoStore;

        public SignInController(
             // ILogger<AuthenticationController> logger,
             IOptions<JwtSettings> jwtSettings,
             AuthInfoStore authInfoStore)
        {
            // _logger = logger;
            _jwtSettings = jwtSettings;
            _authInfoStore = authInfoStore;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(Sign sign)
        {
            if (sign is not null && sign.HasData)
            {
                var user = await GetUser(sign.Email, sign.Password);

                if (user is not null)
                {
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _jwtSettings.Value.Subject), 
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(nameof(user.UserName), user.UserName),
                        new Claim(ClaimTypes.Role, sign.Role),
                        new Claim(nameof(user.CreatedDate), user.CreatedDate.ToString())
                        // new Claim(nameof(user.Password), user.Password) // TODO: password入れないっぽい?
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Key));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _jwtSettings.Value.Issuer,
                        _jwtSettings.Value.Audience, 
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(60),
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">名前(メールアドレス)</param>
        /// <param name="passwordFromRequest"></param>
        /// <returns></returns>
        private async Task<AuthInfo?> GetUser(string name, string passwordFromRequest)
        {
            var authInfo = await _authInfoStore.FindByNameAsync(name, default);
            if (authInfo is not null)
            {
                var haser = new PasswordHasher<AuthInfo>();
                var result = haser.VerifyHashedPassword(authInfo, authInfo.PasswordHash, passwordFromRequest);
                if (result == PasswordVerificationResult.Success)
                {
                    return authInfo;
                }
            }
            return null;
        }
    }
}
