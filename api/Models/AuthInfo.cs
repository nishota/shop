using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Models
{
    /// <summary>
    /// 認証で利用する情報
    /// メールアドレスをNameとして利用する
    /// </summary>
    public class AuthInfo
    //: IdentityUser<string> // TODO: だめだった
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        /// <summary>
        /// メールアドレス
        /// (実装では、Nameとして扱う)
        /// </summary>
        // TODO: 登録したアドレスが一意になるように実装を追加する
        [BsonElement("email")]
        public string UserName { get; set; } = null!;

        /// <summary>
        /// メールアドレスの小文字変換
        /// (実装では、NormalizedNameとして扱う)
        /// </summary>
        // TODO: 登録したアドレスが一意になるように実装を追加する
        [BsonElement("normalizedEmail")]
        public string NormalizedUserName { get; set; } = null!;

        [BsonElement("password")]
        public string PasswordHash { get; set; } = null!;

        [BsonElement("role")]
        public string Role { get; set; } = null!;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [BsonElement("createdDate")]
        public DateTime CreatedDate { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [BsonElement("updatedDate")]
        public DateTime? UpdatedDate { get; set; }

        public AuthInfo GetInstanceWithoutPassword()
        {
            return new AuthInfo
            {
                Id = Id,
                UserName = UserName,
                NormalizedUserName = NormalizedUserName,
                PasswordHash = string.Empty,
                CreatedDate = CreatedDate,
                UpdatedDate = UpdatedDate
            };
        }
    }
}
