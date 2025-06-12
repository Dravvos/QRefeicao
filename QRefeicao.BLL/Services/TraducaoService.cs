using QRefeicao.BLL.Repositories.Interfaces;
using QRefeicao.BLL.Services.Interfaces;
using QRefeicao.DTO;
using System.Net;
using System.Text;
using System.Text.Json;

namespace QRefeicao.BLL.Services
{
    public class TraducaoService : ITraducaoService
    {
        private readonly ITraducaoRepository _repository;

        public TraducaoService(ITraducaoRepository repository)
        {
            _repository = repository;
        }

        private void ValidarDTO(TraducaoDTO dto)
        {
            if (string.IsNullOrEmpty(dto.IdiomaTraduzido))
                throw new ArgumentNullException("Digite/Selecione o idioma de destino da tradução");

            if (string.IsNullOrEmpty(dto.IdiomaOriginal))
                throw new ArgumentNullException("Digite/Selecione o idioma de origem da tradução");

            if (string.IsNullOrEmpty(dto.TextoOriginal))
                throw new ArgumentNullException("Digite texto a ser traduzido");
        }

        public async Task CreateTraducao(TraducaoDTO dto)
        {
            ValidarDTO(dto);

            var client = new HttpClient();

            var json = JsonSerializer.Serialize(new
            {
                source = dto.IdiomaOriginal.ToLower(),
                target = dto.IdiomaTraduzido.ToLower() !="en" && dto.IdiomaTraduzido.ToLower()!="es" ? "en" : dto.IdiomaTraduzido.ToLower(),
                q = dto.TextoOriginal
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("http://145.223.95.97:6000/translate", content);

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created)
                throw new Exception("Não foi possível obter tradução do termo: " + dto.TextoOriginal);

            var res = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(res))
                throw new Exception("Não foi possível obter tradução do termo: " + dto.TextoOriginal);

            var jsonRes = JsonSerializer.Deserialize<Translate>(res);

            if (dto.IdiomaTraduzido.ToLower() != "en" && dto.IdiomaTraduzido.ToLower() != "es")
            {
                json = JsonSerializer.Serialize(new
                {
                    source = "en",
                    target = dto.IdiomaTraduzido.ToLower(),
                    q = jsonRes.translatedText
                });

                content = new StringContent(json, Encoding.UTF8, "application/json");
                response = await client.PostAsync("http://145.223.95.97:6000/translate", content);

                if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created)
                    throw new Exception("Não foi possível obter tradução do termo: " + dto.TextoOriginal);

                res = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(res))
                    throw new Exception("Não foi possível obter tradução do termo: " + dto.TextoOriginal);

                jsonRes = JsonSerializer.Deserialize<Translate>(res);
            }

            dto.TextoTraduzido = jsonRes.translatedText;

            dto.DataInclusao = DateTime.UtcNow;
            await _repository.CreateTraducao(dto);
        }

        public async Task<string?> GetTraducao(string texto, string idiomaTraduzido)
        {
            return await _repository.GetTraducao(texto, idiomaTraduzido);
        }

        public async Task<IList<TraducaoDTO>> GetTraducoes(string idiomaOriginal, string idiomaTraduzido)
        {
            return await _repository.GetTraducoes(idiomaOriginal, idiomaTraduzido);
        }

        public async Task UpdateTraducao(TraducaoDTO dto)
        {
            ValidarDTO(dto);

            dto.DataAlteracao = DateTime.UtcNow;
            await _repository.UpdateTraducao(dto);
        }
    }

    internal class Translate
    {
        public string? translatedText { get; set; }
    }
}
