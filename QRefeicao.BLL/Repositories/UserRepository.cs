using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QRefeicao.BLL.Repositories.Interfaces;
using QRefeicao.Identity.Models;

namespace QRefeicao.BLL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthContext con;

        public UserRepository(AuthContext con)
        {
            this.con = con;
        }

        public async Task<Guid> GetUserId(string email)
        {
            var user = await con.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
                user = await con.Users.FirstOrDefaultAsync(x => x.UserName == email);

            Guid.TryParse(user.Id, out Guid userId);
            return userId;
        }
    }
}
