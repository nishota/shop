using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace api.Models
{
    public class ItemInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Name { get; set; } = null!;
        public string Discription { get; set; } = null!;
        // TODO: 商品画像[]
        public IEnumerable<StockInfo> Stocks { get; set; } = null!;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? Deadline { get; set; }
        [BsonRepresentation(BsonType.String)]
        public ItemStatus Status { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CreatedDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? UpdatedDate { get; set; }
    }
}
