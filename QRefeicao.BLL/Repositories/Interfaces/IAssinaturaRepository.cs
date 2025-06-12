using QRefeicao.DTO;

namespace QRefeicao.BLL.Repositories.Interfaces
{
    public interface IAssinaturaRepository
    {
        Task CreateAssinatura(AssinaturaDTO assinatura);
        Task UpdateAssinatura(AssinaturaDTO assinatura);
        Task DeleteAssinatura(Guid id);
        Task<AssinaturaDTO> GetAssinaturaByUserId(Guid usuarioId);
    }
}
