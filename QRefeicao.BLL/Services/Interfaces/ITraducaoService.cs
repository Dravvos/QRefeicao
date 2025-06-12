using QRefeicao.DTO;

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
