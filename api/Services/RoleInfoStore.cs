using api.Models;
using api.Models.Settings;
using api.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace api.Services
{
    public class RoleInfoStore
        : DatabaseConnectionService
        , IRoleInfoStore
    {
        // TODO: Collectionが見つからなかったとき
        private IMongoCollection<RoleInfo> collection => base.Database.GetCollection<RoleInfo>(DataBaseSettings.RoleCollectionName);
        // TODO: ログ出力実装
        // private readonly ILogger _logger;

        public RoleInfoStore(
            IOptions<DataBaseSettings> databaseSettings)
            : base(databaseSettings)
        {
        }
        public Task<IdentityResult> CreateAsync(RoleInfo role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(RoleInfo role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<RoleInfo> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<RoleInfo> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedRoleNameAsync(RoleInfo role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(RoleInfo role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(RoleInfo role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(RoleInfo role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(RoleInfo role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(RoleInfo role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
