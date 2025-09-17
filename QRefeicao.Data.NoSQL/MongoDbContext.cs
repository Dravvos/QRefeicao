using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using QRefeicao.Data.NoSQL.Models;
using System.Security.Cryptography.X509Certificates;

namespace QRefeicao.Data.NoSQL
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly ILogger<MongoDbContext> _logger;

        public MongoDbContext(ILogger<MongoDbContext> logger)
        {
            _logger = logger;

            var mongoConnectionString = Environment.GetEnvironmentVariable("MongoDBConnection");
            if (string.IsNullOrWhiteSpace(mongoConnectionString))
            {
                _logger.LogInformation("Sem string de conexão");
                Console.WriteLine("Sem string de conexão");
            }
            var settings = MongoClientSettings.FromUrl(new MongoUrl(mongoConnectionString));
            settings.ConnectTimeout = TimeSpan.FromSeconds(10);
            
            var client = new MongoClient(settings);

            _database = client.GetDatabase("QRefeicao");
        }

        public IMongoCollection<TRADUCAODTC_NOSQL> Traducao => _database.GetCollection<TRADUCAODTC_NOSQL>("Traducao");
    }
}
