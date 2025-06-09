using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRefeicao.BLL.Services.Interfaces;
using QRefeicao.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace QRefeicao.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class RestauranteController : ControllerBase
    {
        private readonly IRestauranteService _service;
        private readonly IRestauranteIdiomaService _restauranteIdiomaService;

        public RestauranteController(IRestauranteService service, IRestauranteIdiomaService restauranteIdiomaService)
        {
            _service = service;
            _restauranteIdiomaService = restauranteIdiomaService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetByUser()
        {
            try
            {
                HttpContext.Request.Cookies.TryGetValue("AuthToken", out var cookie);

                if (string.IsNullOrEmpty(cookie))
                    return Unauthorized();

                var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(cookie);
                var claims = decodedToken.Claims;

                var usuarioId = Guid.Parse(claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value);
                if (usuarioId == Guid.Empty)
                    return UnprocessableEntity("Id do usuário não pode ser vazio");

                var restaurante = await _service.GetRestauranteByUserId(usuarioId);
                if (restaurante == null)
                    return NotFound();

                return Ok(restaurante);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return StatusCode(500, ex.InnerException.Message);

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] RestauranteDTO dto)
        {
            try
            {
                if (dto == null)
                    return UnprocessableEntity("Restaurante não pode estar nulo");

                dto.UsuarioInclusao = User.FindFirstValue(JwtRegisteredClaimNames.Name);
                await _service.CreateRestaurante(dto);
                return StatusCode(201);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return StatusCode(500, ex.InnerException.Message);

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Put(Guid id, [FromBody] RestauranteDTO dto)
        {
            try
            {
                if (dto == null)
                    return UnprocessableEntity("Restaurante não pode estar nulo");

                if (id != dto.Id)
                    return BadRequest("Id do restaurante não confere");

                dto.UsuarioAlteracao = User.FindFirstValue(JwtRegisteredClaimNames.Name);
                await _service.UpdateRestaurante(dto);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return StatusCode(500, ex.InnerException.Message);

                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _service.DeleteRestaurante(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return StatusCode(500, ex.InnerException.Message);

                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Idioma/{restauranteId:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetIdiomas(Guid restauranteId)
        {
            try
            {
                if (restauranteId == Guid.Empty)
                    return UnprocessableEntity();

                var dtos = await _restauranteIdiomaService.GetIdiomasRestaurante(restauranteId);
                if (dtos == null || dtos.Any() == false)
                    return NotFound();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return StatusCode(500, ex.InnerException.Message);

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Idioma")]
        [Authorize]
        public async Task<IActionResult> AssignIdioma([FromBody] List<RestauranteIdiomaDTO> dtos)
        {
            try
            {
                if (dtos == null || dtos.Any() == false)
                    return UnprocessableEntity();

                var idiomasDto = dtos.Select(x => x.IdTGIdioma).ToList();
                var idiomasRestaurante = await _restauranteIdiomaService.GetIdiomasRestaurante(dtos[0].RestauranteId);
                if (idiomasRestaurante.Any() && idiomasRestaurante.All(x => idiomasDto.Contains(x.IdTGIdioma))) //Verifica se os idiomas salvos estão presentes no DTO
                {

                    foreach (var dto in dtos)
                    {
                        if (dto == null)
                            return UnprocessableEntity();

                        dto.UsuarioAlteracao = User.FindFirstValue(JwtRegisteredClaimNames.Name);
                    }
                    await _restauranteIdiomaService.Update(dtos);
                    return NoContent();
                }
                else
                {
                    foreach (var dto in dtos)
                    {
                        if (dto == null)
                            return UnprocessableEntity();

                        dto.UsuarioInclusao = User.FindFirstValue(JwtRegisteredClaimNames.Name);
                    }
                    await _restauranteIdiomaService.Create(dtos);
                    return StatusCode(201);
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return StatusCode(500, ex.InnerException.Message);

                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("Idioma/{id:guid}")]
        public async Task<IActionResult> DeleteIdioma(Guid id)
        {
            try
            {
                await _restauranteIdiomaService.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return StatusCode(500, ex.InnerException.Message);

                return StatusCode(500, ex.Message);
            }
        }
    }
}
