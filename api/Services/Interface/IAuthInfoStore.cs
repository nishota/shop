using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Services.Interface
{
    public interface IAuthInfoStore
        : IUserStore<AuthInfo>
        , IUserEmailStore<AuthInfo>
        , IUserPasswordStore<AuthInfo>
    {
    }
}
