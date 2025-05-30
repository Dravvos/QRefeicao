using Microsoft.EntityFrameworkCore;
using QRefeicao.Data.Models;

namespace QRefeicao.Data
{
    public class QRContext:DbContext
    {
        public QRContext()
        {
            
        }
        public QRContext(DbContextOptions<QRContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var con = Environment.GetEnvironmentVariable("QRCon");
            optionsBuilder.UseNpgsql(con);
        }

        public DbSet<TabelaGeralModel> TabelaGeral { get; set; }
        public DbSet<TabelaGeralItemModel> TabelaGeralItem { get; set; }
        public DbSet<AssinaturaModel> Assinatura { get; set; }
        public DbSet<CategoriaModel> Categoria { get; set; }
        public DbSet<CardapioModel> Cardapio { get; set; }
        public DbSet<CardapioItemModel> CardapioItem { get; set; }
        public DbSet<RestauranteModel> Restaurante { get; set; }

    }
}
