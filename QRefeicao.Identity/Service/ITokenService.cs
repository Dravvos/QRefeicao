using Microsoft.AspNetCore.Identity;
using QRefeicao.Identity.Models;

namespace QRefeicao.Identity.Service
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager);
    }
}
