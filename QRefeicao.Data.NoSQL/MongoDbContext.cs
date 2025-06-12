using MongoDB.Driver;
using QRefeicao.Data.NoSQL.Models;
using System.Security.Cryptography.X509Certificates;

namespace QRefeicao.Data.NoSQL
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext()
        {
            var mongoConnectionString = Environment.GetEnvironmentVariable("MongoDBConnection");
            var settings = MongoClientSettings.FromUrl(new MongoUrl(mongoConnectionString));

            var caCert = new X509Certificate2(@"C:\Users\supero\mongodb-ca.crt");
            var clientCert = new X509Certificate2(@"C:\Users\supero\mongodb-client.pfx", "YqY,&soTB_fQ!r5#",
                 X509KeyStorageFlags.MachineKeySet |
            X509KeyStorageFlags.PersistKeySet |
            X509KeyStorageFlags.Exportable);

            settings.SslSettings = new SslSettings
            {
                ClientCertificates = new List<X509Certificate> { clientCert },
                ServerCertificateValidationCallback = (sender, cert, chain, errors) => true
            };

            var client = new MongoClient(settings);

            _database = client.GetDatabase("QRefeicao");
        }

        public IMongoCollection<TRADUCAODTC_NOSQL> Traducao => _database.GetCollection<TRADUCAODTC_NOSQL>("Traducao");
    }
}
