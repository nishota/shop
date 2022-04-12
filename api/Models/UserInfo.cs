using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Models
{
    public class UserInfo
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId UserId { get; set; }
        public string Name { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CreatedDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? UpdatedDate { get; set; }

        // TODO: 郵便番号(postal code)
        // TODO: 住所
        // TODO: 電話番号
        // TODO: 生年月日(要らないかも)
        // TODO: プロフィール画像バイナリ
        // TODO: 買い物カート商品[]

    }
}
