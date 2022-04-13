using api.Models.Settings;
using api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace api.Services
{
    public class AuthInfoService
        : DatabaseConnectionService
    {
        private readonly IMongoCollection<AuthInfo> _collection;
        // TODO: ログ出力実装
        // private readonly ILogger _logger;

        public AuthInfoService(
            IOptions<DataBaseSettings> databaseSettings)
            : base(databaseSettings)
        {
            this._collection = base.Database.GetCollection<AuthInfo>(DataBaseSettings.AuthCollectionName);
            // TODO: 指定のColllectionがなかった場合
            // if (this._collection == null) throw new HogeHogeException();
        }

        /// <summary>
        /// ObjectIdで検索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AuthInfo?> GetAsync(ObjectId id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        /// <summary>
        /// メールアドレスで検索
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<AuthInfo?> GetAsync(string email) =>
            await _collection.Find(x => x.Email == email).FirstOrDefaultAsync();
        /// <summary>
        /// メールアドレスとパスワードで検索
        /// </summary>
        /// <param name="email">メールアドレス</param>
        /// <param name="encryptedPassword">暗号化されたパスワード</param>
        /// <returns></returns>
        public async Task<AuthInfo?> GetAsync(string email, string encryptedPassword) =>
            await _collection.Find(x => x.Email == email && x.Password == encryptedPassword).FirstOrDefaultAsync();

        public async Task CreateAsync(AuthInfo userInfo) =>
            await _collection.InsertOneAsync(userInfo);

        public async Task UpdateAsync(ObjectId id, AuthInfo updatedUserInfo) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, updatedUserInfo);

        public async Task RemoveAsync(ObjectId id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }
}
