using QRefeicao.DTO;

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
