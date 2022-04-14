using api.Models;
using api.Models.Schemas;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly AuthInfoStore _authInfoStore;

        public SignUpController(
             // ILogger<AuthenticationController> logger,
             AuthInfoStore authInfoStore)
        {
            // _logger = logger;
            _authInfoStore = authInfoStore;
        }

        // TODO: リクエストを送るほうで権限を決めるので、そこを修正したい
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(Sign sign)
        {
            if(sign is not null && sign.HasData)
            {
                var hasEmail = await HasAuthInfo(sign.Email);
                if(hasEmail)
                {
                    return BadRequest("Failed to create user. Your email address is already registered.");
                }
                var newAuthInfo = await CreateUser(sign);
                if (newAuthInfo is not null) return Ok(newAuthInfo);

                return BadRequest("Failed to create user.");
            }

            throw new ArgumentNullException(nameof(sign));
        }


        private async Task<bool> HasAuthInfo(string email)
        {
            var user = await _authInfoStore.FindByNameAsync(email, default);
            return user is not null;
        }

        private async Task<AuthInfo?> CreateUser(Sign sign)
        {
            // TODO: emailがフォーマットに合っているか
            //       front側でもチェック予定だが、念のため実装しておきたい
            var haser = new PasswordHasher<AuthInfo>();
            var newAuthInfo = new AuthInfo
            {
                Id = ObjectId.GenerateNewId(),
                UserName = sign.Email,
                NormalizedUserName = sign.Email.ToLower(),
                Role = sign.Role,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };
            newAuthInfo.PasswordHash = haser.HashPassword(newAuthInfo, sign.Password);
            var result = await _authInfoStore.CreateAsync(newAuthInfo, default);
            if(result.Succeeded) return await Task.FromResult(newAuthInfo.GetInstanceWithoutPassword());
            return null;
        }
    }
}
