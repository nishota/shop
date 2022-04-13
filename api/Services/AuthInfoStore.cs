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

        ///// <summary>
        ///// ObjectIdで検索
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public async Task<AuthInfo?> GetAsync(ObjectId id) =>
        //    await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        ///// <summary>
        ///// メールアドレスで検索
        ///// </summary>
        ///// <param name="email"></param>
        ///// <returns></returns>
        //public async Task<AuthInfo?> GetAsync(string email) =>
        //    await _collection.Find(x => x.Email == email).FirstOrDefaultAsync();
        ///// <summary>
        ///// メールアドレスとパスワードで検索
        ///// </summary>
        ///// <param name="email">メールアドレス</param>
        ///// <param name="encryptedPassword">暗号化されたパスワード</param>
        ///// <returns></returns>
        //public async Task<AuthInfo?> GetAsync(string email, string encryptedPassword) =>
        //    await _collection.Find(x => x.Email == email && x.Password == encryptedPassword).FirstOrDefaultAsync();

        public async Task ReplaceAuthInfoAsync(AuthInfo authInfo ,CancellationToken cancellationToken)
        {
            await Collection.ReplaceOneAsync(
                x => x.Id == authInfo.Id
                , replacement: authInfo
                , cancellationToken: cancellationToken);
        }

        public async Task SetEmailAsync(AuthInfo user, string email, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            if(email is null) throw new ArgumentNullException(nameof(email));

            var updatedUser = new AuthInfo
            {
                Id = user.Id,
                Name = user.Name,
                NormalizedName = user.NormalizedName,
                Email = email,
                IsEmailConfirmed = user.IsEmailConfirmed,
                Password = user.Password,
                UpdatedDate = user.UpdatedDate,
                CreatedDate = user.CreatedDate
            };
            await ReplaceAuthInfoAsync(updatedUser, cancellationToken);
        }

        public async Task<string> GetEmailAsync(AuthInfo user, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            var userFromDB = await FindByIdAsync(user.Id, cancellationToken);
            if(userFromDB is not null)
            {
                return await Task.FromResult(userFromDB.Email);
            }
            throw new ArgumentNullException(nameof(user));
        }

        public async Task<bool> GetEmailConfirmedAsync(AuthInfo user, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            var userFromDB = await FindByIdAsync(user.Id, cancellationToken);
            if (userFromDB is not null)
            {
                return await Task.FromResult(userFromDB.IsEmailConfirmed);
            }
            throw new ArgumentNullException(nameof(user));
        }

        public async Task SetEmailConfirmedAsync(AuthInfo user, bool confirmed, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));

            var updatedUser = new AuthInfo
            {
                Id = user.Id,
                Name = user.Name,
                NormalizedName = user.NormalizedName,
                Email = user.Email,
                IsEmailConfirmed = confirmed,
                Password = user.Password,
                UpdatedDate = user.UpdatedDate,
                CreatedDate = user.CreatedDate
            };
            await ReplaceAuthInfoAsync(updatedUser, cancellationToken);
        }

        public async Task<AuthInfo> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await Collection.Find(x => x.NormalizedEmail == normalizedEmail).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<string> GetNormalizedEmailAsync(AuthInfo user, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            var userFromDB = await FindByIdAsync(user.Id, cancellationToken);
            if (userFromDB is not null)
            {
                return await Task.FromResult(userFromDB.NormalizedEmail);
            }
            throw new ArgumentNullException(nameof(user));
        }

        public async Task SetNormalizedEmailAsync(AuthInfo user, string normalizedEmail, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            if(normalizedEmail == null) throw new ArgumentNullException(nameof(normalizedEmail));

            var updatedUser = new AuthInfo
            {
                Id = user.Id,
                Name = user.Name,
                NormalizedName = user.NormalizedName,
                Email = normalizedEmail,
                IsEmailConfirmed = user.IsEmailConfirmed,
                Password = user.Password,
                UpdatedDate = user.UpdatedDate,
                CreatedDate = user.CreatedDate
            };
            await ReplaceAuthInfoAsync(updatedUser, cancellationToken);
        }

        public async Task SetPasswordHashAsync(AuthInfo user, string passwordHash, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            if (passwordHash == null) throw new ArgumentNullException(nameof(passwordHash));

            var updatedUser = new AuthInfo
            {
                Id = user.Id,
                Name = user.Name,
                NormalizedName = user.NormalizedName,
                Email = user.Email,
                IsEmailConfirmed = user.IsEmailConfirmed,
                Password = passwordHash,
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
                return await Task.FromResult(userFromDB.Password);
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
            if (user is null) throw new ArgumentNullException(nameof(user));
            var userFromDB = await FindByIdAsync(user.Id, cancellationToken);
            if (userFromDB is not null)
            {
                return await Task.FromResult(userFromDB.Id.ToString());
            }
            throw new ArgumentNullException(nameof(user));
        }

        public async Task<string> GetUserNameAsync(AuthInfo user, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            var userFromDB = await FindByIdAsync(user.Id, cancellationToken);
            if (userFromDB is not null)
            {
                return await Task.FromResult(userFromDB.Name);
            }
            throw new ArgumentNullException(nameof(user));
        }

        public async Task SetUserNameAsync(AuthInfo user, string userName, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            if (userName == null) throw new ArgumentNullException(nameof(userName));

            var updatedUser = new AuthInfo
            {
                Id = user.Id,
                Name = userName,
                NormalizedName = user.NormalizedName,
                Email = user.Email,
                IsEmailConfirmed = user.IsEmailConfirmed,
                Password = user.Password,
                UpdatedDate = user.UpdatedDate,
                CreatedDate = user.CreatedDate
            };
            await ReplaceAuthInfoAsync(updatedUser, cancellationToken);
        }
        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<string> GetNormalizedUserNameAsync(AuthInfo user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="normalizedName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SetNormalizedUserNameAsync(AuthInfo user, string normalizedName, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            if (normalizedName == null) throw new ArgumentNullException(nameof(normalizedName));

            var updatedUser = new AuthInfo
            {
                Id = user.Id,
                Name = user.Name,
                NormalizedName = normalizedName,
                Email = user.Email,
                IsEmailConfirmed = user.IsEmailConfirmed,
                Password = user.Password,
                UpdatedDate = user.UpdatedDate,
                CreatedDate = user.CreatedDate
            };
            await ReplaceAuthInfoAsync(updatedUser, cancellationToken);
        }

        public async Task<IdentityResult> CreateAsync(AuthInfo user, CancellationToken cancellationToken)
        {
            await Collection.InsertOneAsync(user, cancellationToken: cancellationToken);
            return await Task.FromResult(IdentityResult.Success); //TODO;失敗したとき
        }

        public async Task<IdentityResult> UpdateAsync(AuthInfo user, CancellationToken cancellationToken)
        {
            await Collection.ReplaceOneAsync(x => x.Id == user.Id, user, cancellationToken:cancellationToken);
            return await Task.FromResult(IdentityResult.Success); //TODO;失敗したとき
        }

        public async Task<IdentityResult> DeleteAsync(AuthInfo user, CancellationToken cancellationToken)
        {
            await Collection.DeleteOneAsync(x => x.Id == user.Id, cancellationToken);
            return await Task.FromResult(IdentityResult.Success); //TODO;失敗したとき
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
            return await Collection.Find(x => x.NormalizedEmail == normalizedUserName).FirstOrDefaultAsync(cancellationToken);
        }

        public void Dispose()
        {
        }
    }
}
