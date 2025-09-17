using QRefeicao.BLL.Repositories.Interfaces;
using QRefeicao.DTO;
using QRefeicao.Data.NoSQL;
using QRefeicao.Data.NoSQL.Models;
using MongoDB.Driver;

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
            if(con is null || con.Traducao is null)
                throw new ArgumentNullException("Conexão com banco de dados não estabelecida");
            if(dto is null)
                throw new ArgumentNullException("DTO nulo");
            await con.Traducao.InsertOneAsync(dtc);
        }

        public async Task<string?> GetTraducao(string texto, string idiomaTraduzido)
        {
            if (con is null || con.Traducao is null)
                throw new ArgumentNullException("Conexão com banco de dados não estabelecida");
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
            if (con is null || con.Traducao is null)
                throw new ArgumentNullException("Conexão com banco de dados não estabelecida");
            var dict = new Dictionary<string, object>();
            dict.Add("IdiomaOriginal", idiomaOriginal);
            dict.Add("IdiomaTraduzido", idiomaTraduzido);
            var filtro = FilterHelper<TRADUCAODTC_NOSQL>.BuildCombinedFilter(dict);
            var traducoes = await con.Traducao.FindAsync(filtro);
            return Map<List<TraducaoDTO>>.Convert(traducoes);
        }

        public async Task UpdateTraducao(TraducaoDTO dto)
        {
            if (con is null || con.Traducao is null)
                throw new ArgumentNullException("Conexão com banco de dados não estabelecida");
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
