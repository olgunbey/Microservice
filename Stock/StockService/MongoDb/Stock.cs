using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StockService.MongoDb
{
    public class Stock
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }
}
