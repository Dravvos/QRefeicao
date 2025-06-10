using MongoDB.Driver;
using QRevfeicao.Data.NoSQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            var settings = MongoClientSettings.FromUrl(new MongoUrl(mongoConnectionString));

            var caCert = new X509Certificate2(@"C:\Users\Daniel\mongodb-ca.crt");
            var clientCert = new X509Certificate2(@"C:\Users\Daniel\mongodb-client.pfx", "YqY,&soTB_fQ!r5#",
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
