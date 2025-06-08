using MongoDB.Bson.Serialization.Conventions;
using QRefeicao.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRefeicao.BLL.Repositories.Interfaces
{
    public interface ITraducaoRepository
    {
        Task<IList<TraducaoDTO>> GetTraducoes(string idiomaOriginal, string idiomaTraduzido);
        Task<string?> GetTraducao(string texto, string idiomaTraduzido);
        Task CreateTraducao(TraducaoDTO dto);
        Task UpdateTraducao(TraducaoDTO dto);
    }
}
