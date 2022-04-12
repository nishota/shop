using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Models
{
    public class AuthInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [BsonElement("email")]
        public string Email { get; set; } = null!;
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
