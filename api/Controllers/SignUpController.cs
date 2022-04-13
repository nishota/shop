using api.Models;
using api.Models.Schemes;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace api.Controllers
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        // TODO: Log出力
        // private readonly ILogger<AuthenticationController> _logger;
        private readonly AuthInfoStore _authInfoService;

        public SignUpController(
             // ILogger<AuthenticationController> logger,
             AuthInfoStore authInfoService)
        {
            // _logger = logger;
            _authInfoService = authInfoService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(Sign sign)
        {
            if(sign is not null && sign.HasData)
            {
                var isChecked = await HasAuthInfo(sign.Email);
                if(isChecked)
                {
                    return BadRequest("Failed to crreate user. Your email address is already registered.");
                }
                var newAuthInfo = await CreateUser(sign.Email, sign.Password);
                return Ok(newAuthInfo);
            }

            throw new ArgumentNullException(nameof(sign));
        }


        private async Task<bool> HasAuthInfo(string email)
        {
            var user = await _authInfoService.GetAsync(email);
            return user is not null;
        }

        private async Task<AuthInfo?> CreateUser(string email, string password)
        {
            // TODO: passwordの暗号化。
            //       一旦、平文でDBに入れる。
            // TODO: emailがフォーマットに合っているか
            //       front側でもチェック予定だが、念のため実装しておきたい
            var id = ObjectId.GenerateNewId();
            var newAuthInfo = new AuthInfo
            {
                Id = id,
                Email = email,
                Password = password,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };
            await _authInfoService.CreateAsync(newAuthInfo);
            return await Task.FromResult(newAuthInfo);
        }
    }
}
