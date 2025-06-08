using MongoDB.Driver;
using QRevfeicao.Data.NoSQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRevfeicao.Data.NoSQL
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext()
        {
            var mongoConnectionString = Environment.GetEnvironmentVariable("MongoDBConnection");
            var client = new MongoClient(mongoConnectionString);

            _database = client.GetDatabase("QRefeicao");
        }

        public IMongoCollection<TRADUCAODTC_NOSQL> Traducao =>
            _database.GetCollection<TRADUCAODTC_NOSQL>("Traducao");
    }
}
