using api.Models.Settings;
using api.Models;
using api.Services.Interface;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.AspNetCore.Identity;

namespace api.Services
{
    public class AuthInfoStore
        : DatabaseConnectionService
        , IAuthInfoStore
    {
        // TODO: Collectionが見つからなかったとき
        private IMongoCollection<AuthInfo> Collection => base.Database.GetCollection<AuthInfo>(DataBaseSettings.AuthCollectionName);
        // TODO: ログ出力実装
        // private readonly ILogger _logger;

        public AuthInfoStore(
            IOptions<DataBaseSettings> databaseSettings)
            : base(databaseSettings)
        {
        }

        public async Task ReplaceAuthInfoAsync(AuthInfo authInfo ,CancellationToken cancellationToken)
        {
            if (authInfo is null) throw new ArgumentNullException(nameof(authInfo));
            await Collection.ReplaceOneAsync(
                x => x.Id == authInfo.Id
                , replacement: authInfo
                , cancellationToken: cancellationToken);
        }

        public async Task SetPasswordHashAsync(AuthInfo user, string passwordHash, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            if (passwordHash is null) throw new ArgumentNullException(nameof(passwordHash));

            var updatedUser = new AuthInfo
            {
                Id = user.Id,
                UserName = user.UserName,
                NormalizedUserName = user.NormalizedUserName,
                PasswordHash = passwordHash,
                UpdatedDate = user.UpdatedDate,
                CreatedDate = user.CreatedDate
            };
            await ReplaceAuthInfoAsync(updatedUser, cancellationToken);
        }

        public async Task<string> GetPasswordHashAsync(AuthInfo user, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            var userFromDB = await FindByIdAsync(user.Id, cancellationToken);
            if (userFromDB is not null)
            {
                return await Task.FromResult(userFromDB.PasswordHash);
            }
            throw new ArgumentNullException(nameof(user));
        }

        public async Task<bool> HasPasswordAsync(AuthInfo user, CancellationToken cancellationToken)
        {
            var password = await GetPasswordHashAsync(user, cancellationToken);
            return password is not null;
        }

        public async Task<string> GetUserIdAsync(AuthInfo user, CancellationToken cancellationToken)
        {
            var userObjectId = await GetUserObjectIdAsync(user, cancellationToken);
            return await Task.FromResult(userObjectId.ToString());
        }

        public async Task<ObjectId> GetUserObjectIdAsync(AuthInfo user, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            var userFromDB = await FindByIdAsync(user.Id, cancellationToken);
            if (userFromDB is not null)
            {
                return await Task.FromResult(userFromDB.Id);
            }
            throw new ArgumentNullException(nameof(user));
        }

        public async Task<string> GetUserNameAsync(AuthInfo user, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            var userFromDB = await FindByIdAsync(user.Id, cancellationToken);
            if (userFromDB is not null)
            {
                return await Task.FromResult(userFromDB.UserName);
            }
            throw new ArgumentNullException(nameof(user));
        }

        public async Task SetUserNameAsync(AuthInfo user, string userName, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            if (userName is null) throw new ArgumentNullException(nameof(userName));

            var updatedUser = new AuthInfo
            {
                Id = user.Id,
                UserName = userName,
                NormalizedUserName= userName.ToLower(),
                PasswordHash = user.PasswordHash,
                UpdatedDate = user.UpdatedDate,
                CreatedDate = user.CreatedDate
            };
            await ReplaceAuthInfoAsync(updatedUser, cancellationToken);
        }

        public async Task<string> GetNormalizedUserNameAsync(AuthInfo user, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            var userFromDB = await FindByIdAsync(user.Id, cancellationToken);
            if (userFromDB is not null)
            {
                return await Task.FromResult(userFromDB.NormalizedUserName);
            }
            throw new ArgumentNullException(nameof(user));
        }

        public async Task SetNormalizedUserNameAsync(AuthInfo user, string normalizedName, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            if (normalizedName is null) throw new ArgumentNullException(nameof(normalizedName));

            var updatedUser = new AuthInfo
            {
                Id = user.Id,
                UserName = normalizedName, // TODO: 一旦、そのまま入れる。不都合があったら変える。
                NormalizedUserName= normalizedName,
                PasswordHash = user.PasswordHash,
                UpdatedDate = user.UpdatedDate,
                CreatedDate = user.CreatedDate
            };
            await ReplaceAuthInfoAsync(updatedUser, cancellationToken);
        }

        public async Task<IdentityResult> CreateAsync(AuthInfo user, CancellationToken cancellationToken)
        {
            await Collection.InsertOneAsync(user, cancellationToken: cancellationToken);
            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> UpdateAsync(AuthInfo user, CancellationToken cancellationToken)
        {
            await Collection.ReplaceOneAsync(x => x.Id == user.Id, user, cancellationToken:cancellationToken);
            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> DeleteAsync(AuthInfo user, CancellationToken cancellationToken)
        {
            await Collection.DeleteOneAsync(x => x.Id == user.Id, cancellationToken);
            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<AuthInfo> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if(userId is null) throw new ArgumentNullException(nameof(userId));
            return await FindByIdAsync(ObjectId.Parse(userId), cancellationToken);
        }

        public async Task<AuthInfo> FindByIdAsync(ObjectId userId, CancellationToken cancellationToken)
        {
            return await Collection.Find(x => x.Id == userId).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<AuthInfo> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await Collection.Find(x => x.NormalizedUserName == normalizedUserName).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
