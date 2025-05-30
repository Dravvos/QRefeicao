using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<AssinaturaDTO> GetAssinaturaByUserId(Guid usuarioId)
        {
            var assinatura = await con.Assinatura.Include(x => x.TipoAssinatura.TabelaGeral).Include(x => x.StatusAssinatura.TabelaGeral)
                .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);
            return Map<AssinaturaDTO>.Convert(assinatura);
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
