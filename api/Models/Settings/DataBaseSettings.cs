namespace api.Models.Settings
{
    public class DataBaseSettings
    {
        // for MondoDB

        /// <summary>
        /// appsettings.jsonとつなぐための文字列
        /// </summary>
        public const string Database = "ShopDatabaseV1";
        /// <summary>
        /// MongoDBで定義してあるDatabase名
        /// </summary>
        public const string DataBaseName = "FreshVegetableShop";

        public const string AuthCollectionName = "Auth";
        public const string RoleCollectionName = "Role";

        /// <summary>
        /// MongoDBとの接続文字列
        /// </summary>
        public string ConnectionString { get; set; } = null!;
    }
}
