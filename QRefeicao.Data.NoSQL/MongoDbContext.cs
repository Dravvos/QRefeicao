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
        }
        public MongoDbContext()
        {
            try
            {
                var mongoConnectionString = Environment.GetEnvironmentVariable("MongoDBConnection");
                var settings = MongoClientSettings.FromUrl(new MongoUrl(mongoConnectionString));

                string crtPath = @"C:\Users\supero\mongodb-ca.crt";
                string pfxPath = @"C:\Users\supero\mongodb-client.pfx";
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                {
                    crtPath = "/etc/mongodb/certs/mongodb-ca.crt";
                    pfxPath = "/etc/mongodb/certs/mongodb-client.pfx";

                    _logger.LogDebug("Ambiente de produção detectado");
                }
                else
                {
                    _logger.LogDebug("Ambiente de desenvolvimento detectado, utilizando certificados locais.");
                }
                    var caCert = new X509Certificate2(crtPath);
                var clientCert = new X509Certificate2(pfxPath, Environment.GetEnvironmentVariable("CertificatePassword"),
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
            catch (Exception ex)
            {
                _logger.LogError("Erro ao conectar no mongo: " + ex.Message);
            }            
        }

        public IMongoCollection<TRADUCAODTC_NOSQL> Traducao => _database.GetCollection<TRADUCAODTC_NOSQL>("Traducao");
    }
}
