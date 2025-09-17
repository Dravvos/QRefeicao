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
            else
            {
                _logger.LogInformation(mongoConnectionString);
                Console.WriteLine(mongoConnectionString);
            }
            var settings = MongoClientSettings.FromUrl(new MongoUrl(mongoConnectionString));
            settings.ConnectTimeout = TimeSpan.FromSeconds(10);
            string certPath = @"C:\Users\supero\mongodb-ca.crt";
            string pfxPath = @"C:\Users\supero\mongodb-client.pfx";

            var ambiente = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (ambiente == "Production")
            {
                certPath = "/etc/mongodb/certs/mongodb-ca.crt";
                pfxPath = "/etc/mongodb/certs/mongodb-client.pfx";
            }
            var caCert = new X509Certificate2(certPath);

            var clientCert = new X509Certificate2(pfxPath, Environment.GetEnvironmentVariable("CertificatePassword"),
                 X509KeyStorageFlags.MachineKeySet |
            X509KeyStorageFlags.PersistKeySet |
            X509KeyStorageFlags.Exportable);

            var certificationCollection = new X509Certificate2Collection { caCert, clientCert };
            if (ambiente == "Production")
            {
                certificationCollection.Remove(clientCert);
            }
            settings.SslSettings = new SslSettings
            {
                ClientCertificates = certificationCollection,
                ServerCertificateValidationCallback = (sender, cert, chain, errors) => true,
                CheckCertificateRevocation = false,
            };

            var client = new MongoClient(settings);

            _database = client.GetDatabase("QRefeicao");
        }

        public IMongoCollection<TRADUCAODTC_NOSQL> Traducao => _database.GetCollection<TRADUCAODTC_NOSQL>("Traducao");
    }
}
