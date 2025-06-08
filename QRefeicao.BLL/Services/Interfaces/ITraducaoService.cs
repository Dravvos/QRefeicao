using QRefeicao.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRefeicao.BLL.Services.Interfaces
{
    public interface ITraducaoService
    {
        Task<IList<TraducaoDTO>> GetTraducoes(string idiomaOriginal, string idiomaTraduzido);
        Task<string?> GetTraducao(string texto, string idiomaTraduzido);
        Task CreateTraducao(TraducaoDTO dto);
        Task UpdateTraducao(TraducaoDTO dto);
    }
}
