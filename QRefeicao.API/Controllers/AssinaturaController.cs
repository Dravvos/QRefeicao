using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using QRefeicao.BLL.Services.Interfaces;
using QRefeicao.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net.Http.Headers;
using System.Text.Json;

namespace QRefeicao.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssinaturaController : ControllerBase
    {
        private readonly IAssinaturaService _service;
        private readonly ITabelaGeralItemService _tabelaGeralItemService;
        private readonly ITabelaGeralService _tabelaGeralService;

        public AssinaturaController(IAssinaturaService service, ITabelaGeralItemService tabelaGeralItemService, ITabelaGeralService tabelaGeralService)
        {
            _service = service;
            _tabelaGeralItemService = tabelaGeralItemService;
            _tabelaGeralService = tabelaGeralService;
        }

        [HttpGet]
        [Route("[action]/{tipoAssinaturaId}")]
        public async Task<IActionResult> GetCheckoutId(Guid tipoAssinaturaId)
        {
            try
            {
                var ambiente = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                string? apiKey = Environment.GetEnvironmentVariable("Asaas_ApiKey");
                string? uri = ambiente == "Production" ? "https://api.asaas.com/v3/checkouts" : "https://api-sandbox.asaas.com/v3/checkouts";
                var client = new HttpClient();

                decimal valor = 0;

                var tipoAssinatura = await _tabelaGeralItemService.GetByIdAsync(tipoAssinaturaId);

                if (tipoAssinatura.Sigla == "PRM")
                {
                    valor = 199;
                }
                else if (tipoAssinatura.Sigla == "PRO")
                {
                    valor = 99;
                }
                else
                {
                    valor = 49;
                }

                object checkoutObject = new
                {
                    billingTypes = new string[] { "CREDIT_CARD", "PIX" },
                    chargeTypes = new string[] { "DETACHED", "INSTALLMENT" },
                    minutesToExpire = 30,
                    callback = new
                    {
                        successUrl = ambiente != "Production" ? "http://127.0.0.1:5173/QRefeicao/Restaurante/" + tipoAssinaturaId : "https://www.danieloliveira.net.br/QRefeicao/Restaurante/" + tipoAssinaturaId,
                        cancelUrl = ambiente != "Production" ? "http://127.0.0.1:5173/QRefeicao/" : "https://www.danieloliveira.net.br/QRefeicao/",
                        expiredUrl = ambiente != "Production" ? "http://127.0.0.1:5173/QRefeicao/Precos" : "https://www.danieloliveira.net.br/QRefeicao/Precos"
                    },
                    items = new object[]
                    {
                        new
                        {
                            description = "Sistema de gerenciamento de cardápios online",
                            name = "QRefeição",
                            quantity = 1,
                            value = valor
                        }
                    },
                    installments = new
                    {
                        maxInstallmentCount = 3
                    }
                };

                string stringContent = JsonSerializer.Serialize(checkoutObject);

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(uri),
                    Headers =
    {
                    { "accept", "application/json" },
                    { "access_token", apiKey },
                    {"User-Agent",".NetApplication/1.0.0" },
                    {"Access-Control-Allow-Origin","*" }
    },
                    Content = new StringContent(stringContent)
                    {
                        Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("application/json")
                        }
                    }
                };
                using (var response = await client.SendAsync(request))
                {
                    //response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(body);
                    return Ok(body);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    return StatusCode(500, ex.Message);
                return StatusCode(500, ex.InnerException.Message);

            }
        }

        [HttpGet]
        public async Task<IActionResult> GetByUserId()
        {
            try
            {
                HttpContext.Request.Cookies.TryGetValue("AuthToken", out var cookie);

                if (string.IsNullOrEmpty(cookie))
                    return Unauthorized();

                var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(cookie);
                var claims = decodedToken.Claims;

                string userId = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

                if (string.IsNullOrEmpty(userId))
                    userId = User.Claims.FirstOrDefault().Value;

                var usuarioId = Guid.Parse(userId);
                if (usuarioId == Guid.Empty)
                    return UnprocessableEntity("Id do usuário não pode ser vazio");
                var assinatura = await _service.GetAssinaturaByUserId(usuarioId);
                if (assinatura == null)
                    return NotFound();

                return Ok(assinatura);
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    return StatusCode(500, ex.Message);
                return StatusCode(500, ex.InnerException.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssinatura([FromBody] AssinaturaDTO assinatura)
        {
            try
            {
                assinatura.UsuarioInclusao = User.FindFirstValue(JwtRegisteredClaimNames.Name);
                var tgStatusAssinatura = await _tabelaGeralService.GetByNomeAsync("StatusAssinatura");
                var tgiStatusAssinatura = await _tabelaGeralItemService.GetBySiglaAsync(tgStatusAssinatura.Id.Value, "ATV");
                if (assinatura.UsuarioId == Guid.Empty)
                {
                    string userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
                    if (string.IsNullOrEmpty(userId))
                        userId = User.Claims.FirstOrDefault().Value;
                    assinatura.UsuarioId = Guid.Parse(userId);
                }
                if (assinatura == null)
                    return UnprocessableEntity("Assinatura não pode ser nula");
                assinatura.IdTGStatusAssinatura = tgiStatusAssinatura.Id.Value;
                await _service.CreateAssinatura(assinatura);
                return StatusCode(201);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    return StatusCode(500, ex.Message);
                return StatusCode(500, ex.InnerException.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAssinatura(Guid id, [FromBody] AssinaturaDTO assinatura)
        {
            try
            {
                assinatura.UsuarioAlteracao = User.FindFirstValue(JwtRegisteredClaimNames.Name);
                if (id == Guid.Empty)
                    return UnprocessableEntity("Id da assinatura não pode ser vazio");
                if (assinatura == null)
                    return UnprocessableEntity("Assinatura não pode ser nula");
                await _service.UpdateAssinatura(assinatura);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    return StatusCode(500, ex.Message);
                return StatusCode(500, ex.InnerException.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssinatura(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return UnprocessableEntity("Id da assinatura não pode ser vazio");
                await _service.DeleteAssinatura(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    return StatusCode(500, ex.Message);
                return StatusCode(500, ex.InnerException.Message);
            }
        }

        /*
        [HttpPost("Process/{tipoAssinaturaId}")]
        public async Task<IActionResult> ProcessarPagamento([FromBody] MercadoPagoDTO cardForm, Guid tipoAssinaturaId)
        {
            MercadoPagoConfig.AccessToken = "TEST-7537308538793161-041922-98035968fe22dd4906d91066126786e7-706381060";

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = environment == Environments.Development;

            if (isDevelopment == false)
                MercadoPagoConfig.AccessToken = Environment.GetEnvironmentVariable("MercadoPagoSecretKey");

            try
            {
                var requestOptions = new RequestOptions();

                HttpContext.Request.Cookies.TryGetValue("AuthToken", out var cookie);

                if (string.IsNullOrEmpty(cookie))
                    return Unauthorized();

                var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(cookie);
                var claims = decodedToken.Claims;

                var jti = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;

                requestOptions.CustomHeaders.Add("x-idempotency-key", jti);
                var paymentRequest = new PaymentCreateRequest();

                if (cardForm.PaymentType == "bank_transfer")
                {
                    paymentRequest.TransactionAmount = cardForm.FormData.Transaction_Amount;
                    paymentRequest.Description = "Criador de cardápio QR Code";
                    paymentRequest.PaymentMethodId = "pix";
                    paymentRequest.Payer = new PaymentPayerRequest
                    {
                        Email = cardForm.FormData.Payer.Email,
                        FirstName = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.GivenName)?.Value,
                        LastName = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.FamilyName)?.Value,
                    };
                }
                else
                {
                    paymentRequest.TransactionAmount = cardForm.FormData.Transaction_Amount;
                    paymentRequest.Token = cardForm.FormData.Token;
                    paymentRequest.Description = "Criador de cardápio QR Code";
                    paymentRequest.Installments = cardForm.FormData.Installments;
                    paymentRequest.PaymentMethodId = cardForm.FormData.Payment_Method_Id;

                    paymentRequest.Payer = new PaymentPayerRequest
                    {
                        Email = cardForm.FormData.Payer.Email,
                    };
                }

                var client = new PaymentClient();

                Payment payment = await client.CreateAsync(paymentRequest, requestOptions);
                if (payment.Status.ToUpper().Trim() == "APPROVED")
                    payment = await client.CaptureAsync(payment.Id ?? 0);
                else if (payment.Status.ToUpper().Trim() == "PENDING")
                    return Ok(payment);

                if (payment.Status.ToUpper().Trim() == "APPROVED")
                {
                    var usuarioId = Guid.Parse(claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value);
                    var assinaturaExistente = await _service.GetAssinaturaByUserId(usuarioId);

                    var tgStatusAssinatura = await _tabelaGeralService.GetByNomeAsync("StatusAssinatura");
                    var statusAssinatura = await _tabelaGeralItemService.GetBySiglaAsync(tgStatusAssinatura.Id.GetValueOrDefault(), "ATV");
                    if (assinaturaExistente == null)
                    {
                        await _service.CreateAssinatura(new AssinaturaDTO
                        {
                            DataInicio = DateTime.UtcNow.Date,
                            DataFim = DateTime.UtcNow.Date.AddDays(30),
                            UsuarioId = usuarioId,
                            IdTGTipoAssinatura = tipoAssinaturaId,
                            IdTGStatusAssinatura = statusAssinatura.Id.GetValueOrDefault(),
                            UsuarioInclusao = User.FindFirstValue(JwtRegisteredClaimNames.Name)
                        });
                    }
                    else
                    {
                        assinaturaExistente.IdTGStatusAssinatura = statusAssinatura.Id.GetValueOrDefault();
                        assinaturaExistente.IdTGTipoAssinatura = tipoAssinaturaId;
                        assinaturaExistente.DataInicio = DateTime.UtcNow.Date.ToUniversalTime();
                        assinaturaExistente.DataFim = DateTime.UtcNow.Date.AddDays(30);
                        assinaturaExistente.UsuarioAlteracao = User.FindFirstValue(JwtRegisteredClaimNames.Name);
                        await _service.UpdateAssinatura(assinaturaExistente);

                    }
                }
                return Ok(payment.Status);
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    return StatusCode(500, ex.Message);
                return StatusCode(500, ex.InnerException.Message);

            }
        }
        */
    }
}
