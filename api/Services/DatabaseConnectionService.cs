using api.Models.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace api.Services
{
    public abstract class DatabaseConnectionService
    {
        protected IMongoDatabase Database { get; private set; }
        // TODO: ログ出力実装
        // private readonly ILogger _logger;
        public DatabaseConnectionService(
            IOptions<DataBaseSettings> databaseSettings)
        {
            var settings = MongoClientSettings.FromConnectionString(
                databaseSettings.Value.ConnectionString);
            var client = new MongoClient(settings);
            // 指定のDBを自動で作ってくれるため、存在確認不要
            this.Database = client.GetDatabase(DataBaseSettings.DataBaseName);
        }

        public void Dispose()
        {
        }
    }
}
