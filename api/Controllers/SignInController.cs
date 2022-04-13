using api.Models;
using api.Models.Settings;
using api.Models.Schemes;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Controllers
{
    [ApiController]
    [Route("api/auth/[controller]")]
    public class SignInController : ControllerBase
    {
        // TODO: Log出力
        // private readonly ILogger<AuthenticationController> _logger;
        private readonly IOptions<JwtSettings> _jwtSettings;
        private readonly AuthInfoStore _authInfoService;

        public SignInController(
             // ILogger<AuthenticationController> logger,
             IOptions<JwtSettings> jwtSettings,
             AuthInfoStore authInfoService)
        {
            // _logger = logger;
            _jwtSettings = jwtSettings;
            _authInfoService = authInfoService;
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
                    //create claims details based on the user information
                    var claims = new[] {
                        // TODO: これなに?
                        new Claim(JwtRegisteredClaimNames.Sub, _jwtSettings.Value.Subject), 
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(nameof(user.Email), user.Email),
                        new Claim(nameof(user.CreatedDate), user.CreatedDate.ToString())
                        // new Claim(nameof(user.Password), user.Password) // TODO: password入れないっぽい?
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Key));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        // TODO: これなに?
                        _jwtSettings.Value.Issuer,
                        // TODO: これなに?
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

        private async Task<AuthInfo?> GetUser(string email, string password)
        {
            // TODO: passwordの暗号化。
            //       いったん平文でDBに入れる。
            // TODO: emailがフォーマットに合っているか
            //       front側でもチェック予定だが、念のため実装しておきたい
            return await _authInfoService.GetAsync(email, password);
        }
    }
}
