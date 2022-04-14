using api.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;

namespace api.Services.Interface
{
    public interface IAuthInfoStore
        : IUserStore<AuthInfo>
        , IUserPasswordStore<AuthInfo>
    {
        Task<ObjectId> GetUserObjectIdAsync(AuthInfo user, CancellationToken cancellationToken);
        Task<AuthInfo> FindByIdAsync(ObjectId userId, CancellationToken cancellationToken);
    }
}
