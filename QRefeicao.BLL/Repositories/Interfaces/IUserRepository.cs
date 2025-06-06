using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRefeicao.BLL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<Guid> GetUserId(string email);
    }
}
