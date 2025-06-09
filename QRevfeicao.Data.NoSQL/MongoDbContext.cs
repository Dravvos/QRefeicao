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

            var caCert = new X509Certificate2(@"C:\Users\supero\mongodb-ca.crt");
            var certificadoCliente = File.ReadAllBytes(@"C:\Users\supero\mongodb-client.pem");
            var clientCert = new X509Certificate(certificadoCliente);

            settings.SslSettings = new SslSettings
            {
                ClientCertificates = new List<X509Certificate> { clientCert },
                ServerCertificateValidationCallback = (sender, cert, chain, errors) =>
                {
                    var newChain = new X509Chain();
                    newChain.ChainPolicy.ExtraStore.Add(caCert);
                    newChain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                    return newChain.Build((X509Certificate2)cert);
                }
            };

            var client = new MongoClient(settings);

            _database = client.GetDatabase("QRefeicao");
        }

        public IMongoCollection<TRADUCAODTC_NOSQL> Traducao => _database.GetCollection<TRADUCAODTC_NOSQL>("Traducao");
    }
}
