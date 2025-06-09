using MongoDB.Driver;
using QRefeicao.BLL.Repositories.Interfaces;
using QRefeicao.DTO;
using QRevfeicao.Data.NoSQL;
using QRevfeicao.Data.NoSQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace QRefeicao.BLL.Repositories
{
    public class TraducaoRepository : ITraducaoRepository
    {
        private readonly MongoDbContext con;

        public TraducaoRepository(MongoDbContext con)
        {
            this.con = con;
        }

        public async Task CreateTraducao(TraducaoDTO dto)
        {
            var dtc = Map<TRADUCAODTC_NOSQL>.Convert(dto);
            await con.Traducao.InsertOneAsync(dtc);
        }

        public async Task<string?> GetTraducao(string texto, string idiomaTraduzido)
        {
            try
            {
                var dict = new Dictionary<string, object>();
                dict.Add("TextoOriginal", texto);
                dict.Add("IdiomaTraduzido", idiomaTraduzido);
                var filtro = FilterHelper<TRADUCAODTC_NOSQL>.BuildCombinedFilter(dict);
                var traducao = await con.Traducao.FindAsync(filtro);
                return (await traducao.FirstOrDefaultAsync())?.TextoTraduzido;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public async Task<IList<TraducaoDTO>> GetTraducoes(string idiomaOriginal, string idiomaTraduzido)
        {
            var dict = new Dictionary<string, object>();
            dict.Add("IdiomaOriginal", idiomaOriginal);
            dict.Add("IdiomaTraduzido", idiomaTraduzido);
            var filtro = FilterHelper<TRADUCAODTC_NOSQL>.BuildCombinedFilter(dict);
            var traducoes = await con.Traducao.FindAsync(filtro);
            return Map<List<TraducaoDTO>>.Convert(traducoes);
        }

        public async Task UpdateTraducao(TraducaoDTO dto)
        {
            var filtro = FilterHelper<TRADUCAODTC_NOSQL>.BuildEqualityFilter("Id", dto.Id);

            var updateDefinition = Builders<TRADUCAODTC_NOSQL>.Update
            .Set(e => e.IdiomaTraduzido, dto.IdiomaTraduzido)
            .Set(e => e.IdiomaOriginal, dto.IdiomaOriginal)
            .Set(e => e.TextoTraduzido, dto.TextoTraduzido)
            .Set(e => e.TextoOriginal, dto.TextoOriginal)
            .Set(e => e.DataAlteracao, dto.DataAlteracao);

            await con.Traducao.UpdateOneAsync(filtro, updateDefinition);
        }
    }
}
