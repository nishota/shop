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

        //Login(email, password)
        //メアドとパスワードがDBに存在するか確認する
        // email, passwordの組み合わせが存在する
        // emailは存在するがpasswordが間違い、emailが存在しない
        public async Task<AuthInfo?> GetAsync(string email, string password) =>
            //await _collection.Find(_ => true).FirstOrDefaultAsync();
            await _collection.Find(x => x.Email == email && x.Password == password).FirstOrDefaultAsync();

        public async Task CreateAsync(AuthInfo userInfo) =>
            await _collection.InsertOneAsync(userInfo);

        public async Task UpdateAsync(ObjectId id, AuthInfo updatedUserInfo) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, updatedUserInfo);

        public async Task RemoveAsync(ObjectId id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }
}
