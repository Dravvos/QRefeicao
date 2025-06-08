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
    [Authorize]
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
        public async Task<IActionResult> Create([FromBody] RestauranteDTO dto)
        {
            try
            {
                if (dto == null)
                    return UnprocessableEntity("Restaurante não pode estar nulo");

                dto.UsuarioInclusao = User.FindFirstValue(JwtRegisteredClaimNames.Name);
                await _service.CreateRestaurante(dto);
                return Created();
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

        [HttpGet("idioma/{restauranteId:guid}")]
        public async Task<IActionResult> GetIdiomas(Guid restauranteId)
        {
            try
            {
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

        [HttpPost("idioma")]
        public async Task<IActionResult> AssignIdioma(RestauranteIdiomaDTO dto)
        {
            try
            {
                if (dto == null)
                    return UnprocessableEntity("Restaurante não pode estar nulo");

                dto.UsuarioInclusao = User.FindFirstValue(JwtRegisteredClaimNames.Name);

                await _restauranteIdiomaService.Create(dto);

                return Created();
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

        [HttpPut("idioma/{id:guid}")]
        public async Task<IActionResult> UpdateIdiomas(Guid id, [FromBody] RestauranteIdiomaDTO dto)
        {
            try
            {
                if (dto == null)
                    return UnprocessableEntity("Restaurante não pode estar nulo");

                if (dto.Id != id)
                    return BadRequest();

                dto.UsuarioAlteracao = User.FindFirstValue(JwtRegisteredClaimNames.Name);

                await _restauranteIdiomaService.Update(dto);

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
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

        [HttpDelete("idioma/{id:guid}")]
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
