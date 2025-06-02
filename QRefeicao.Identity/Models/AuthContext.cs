using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace QRefeicao.Identity.Models
{
    public class AuthContext: IdentityDbContext<ApplicationUser>
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {
        }

        public AuthContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = Environment.GetEnvironmentVariable("QRConnection");
            optionsBuilder.UseNpgsql(config);
        }
    }
}
