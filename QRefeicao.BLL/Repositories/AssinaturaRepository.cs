using Microsoft.EntityFrameworkCore;
using QRefeicao.BLL.Repositories.Interfaces;
using QRefeicao.Data;
using QRefeicao.Data.Models;
using QRefeicao.DTO;

namespace QRefeicao.BLL.Repositories
{
    public class AssinaturaRepository : IAssinaturaRepository
    {
        private readonly QRContext con;

        public AssinaturaRepository(QRContext con)
        {
            this.con = con;
        }

        public async Task CreateAssinatura(AssinaturaDTO assinatura)
        {
            var model = Map<AssinaturaModel>.Convert(assinatura);
            await con.Assinatura.AddAsync(model);
            await con.SaveChangesAsync();
        }

        public async Task DeleteAssinatura(Guid id)
        {
            var assinatura = await con.Assinatura.FirstAsync(x => x.Id == id);
            con.Assinatura.Remove(assinatura);
            await con.SaveChangesAsync();
        }

        public async Task<AssinaturaDTO?> GetAssinaturaByUserId(Guid usuarioId)
        {
            var assinatura = await con.Assinatura.AsNoTracking().Where(x => x.UsuarioId == usuarioId)
                .Select(x => new AssinaturaDTO
                {
                    DataInicio = x.DataInicio,
                    DataFim = x.DataFim,
                    StatusAssinatura = new TabelaGeralItemDTO
                    {
                        Id = x.StatusAssinatura.Id,
                        Descricao = x.StatusAssinatura.Descricao,
                        Sigla= x.StatusAssinatura.Sigla,
                    },
                    TipoAssinatura = new TabelaGeralItemDTO
                    {
                        Id = x.TipoAssinatura.Id,
                        Descricao = x.TipoAssinatura.Descricao,
                        Sigla = x.TipoAssinatura.Sigla,
                    }}).FirstOrDefaultAsync();
            return assinatura;
        }

        public async Task UpdateAssinatura(AssinaturaDTO assinatura)
        {

            var model = await con.Assinatura.FirstAsync(x => x.Id == assinatura.Id);
            model.UsuarioId = assinatura.UsuarioId;
            model.DataInicio = assinatura.DataInicio;
            model.DataFim = assinatura.DataFim;
            model.IdTGStatusAssinatura = assinatura.IdTGStatusAssinatura;
            model.IdTGTipoAssinatura = assinatura.IdTGTipoAssinatura;
            model.DataAlteracao = assinatura.DataAlteracao;

            await con.SaveChangesAsync();
        }
    }
}
