using MongoDB.Driver;
using ZstdSharp.Unsafe;

namespace StockService.MongoDb
{
    public class StockMongoService
    {
        private MongoClient _mongo;
        private IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<Stock> _stocks;
        public StockMongoService()
        {
            _mongo = new MongoClient("mongodb+srv://olgunbey:Sahinbey61.@stock.w91kt.mongodb.net/");
            _mongoDatabase = _mongo.GetDatabase("Stock");
            _stocks= _mongoDatabase.GetCollection<Stock>("StockCollection");
        }
        public IMongoCollection<Stock> GetCollection()
        {
            return _stocks;
        }
    }
}
