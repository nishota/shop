using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Models
{
    public class AuthInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Name { get; set; } = null!;
        // TODO: 一意になるように実装を追加する
        public string NormalizedName { get; set; } = null!;
        [BsonElement("email")]

        public string Email { get; set; } = null!;
        public string NormalizedEmail { get; set; } = null!;
        public bool IsEmailConfirmed { get; set; }
        [BsonElement("password")]
        public string Password { get; set; } = null!;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [BsonElement("createdDate")]
        public DateTime CreatedDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [BsonElement("updatedDate")]
        public DateTime? UpdatedDate { get; set; }
    }
}
