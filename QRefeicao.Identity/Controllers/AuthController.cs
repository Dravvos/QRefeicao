using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using QRefeicao.DTO.Auth;
using QRefeicao.Identity.Configuration;
using QRefeicao.Identity.Models;
using QRefeicao.Identity.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QRefeicao.Identity.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _service;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ITokenService service)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _service = service;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAccount([FromBody] SignUpDTO dto, string returnUrl)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
            {
                return BadRequest("Email already in use");
            }

            var result = await _userManager.CreateAsync(new ApplicationUser { Nome = dto.Nome, Sobrenome = dto.Sobrenome, Email = dto.Email, AceitouOsTermosDeUsoPrivacidade = true }, dto.Password);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);
                await _userManager.AddToRoleAsync(user, IdentityConfiguration.Client);
                await _userManager.AddClaimsAsync(user, new Claim[]
              {
                    new Claim(ClaimTypes.Email,dto.Email),
                    new Claim(ClaimTypes.GivenName,dto.Nome),
                    new Claim(ClaimTypes.Surname,dto.Sobrenome),
                    new Claim(ClaimTypes.Role, IdentityConfiguration.Client)
              });

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _userManager.ConfirmEmailAsync(user,token);

                return Created();
            }
            else
                return BadRequest(result.Errors);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(dto.Email);
                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(dto.Email);
                    if (user == null)
                        return BadRequest("Email/Senha inválido(s)");
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, dto.Password, false, false);
                if (result.Succeeded)
                {
                    var token = await _service.GenerateTokenAsync(user, _userManager);
                    var claims = new List<Claim>
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName,user.Nome),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.Sobrenome),
                };
                    var roles = await _userManager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim("role", role));
                        if (_roleManager.SupportsRoleClaims)
                        {
                            var identityRole = await _roleManager.FindByNameAsync(role);
                            if (identityRole != null)
                                claims.AddRange(await _roleManager.GetClaimsAsync(identityRole));
                        }
                    }
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));

                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    var isDevelopment = environment == Environments.Development;
                    var cookieOptions = new CookieOptions();
                    if (isDevelopment == false)
                    {
                        cookieOptions.HttpOnly = true;
                        cookieOptions.Secure = true;
                        cookieOptions.SameSite = SameSiteMode.Lax;
                        cookieOptions.Expires = DateTime.Now.AddHours(3);
                        cookieOptions.IsEssential = true; // Make the session cookie essential
                    }
                    else
                    {
                        cookieOptions.HttpOnly = true;
                        cookieOptions.Secure = true;
                        cookieOptions.SameSite = SameSiteMode.None;
                        cookieOptions.Expires = DateTime.Now.AddHours(3);
                        cookieOptions.IsEssential = true; // Make the session cookie essential
                    }

                    Response.Cookies.Append("AuthToken", token, cookieOptions);
                    return Ok("Logged in succesfully");
                }
                else
                    return BadRequest("Invalid credentials");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }            
        }

        [HttpPut]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var token = HttpContext.Request.Cookies["AuthToken"];
            if (token != null)
            {
                var cookieOptions = new CookieOptions();
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var isDevelopment = environment == Environments.Development;
                if (isDevelopment == false)
                {
                    cookieOptions.HttpOnly = true;
                    cookieOptions.Secure = true;
                    cookieOptions.SameSite = SameSiteMode.Lax;
                    cookieOptions.Expires = DateTime.Now.AddDays(-1);
                    cookieOptions.IsEssential = true; // Make the session cookie essential
                }
                else
                {
                    cookieOptions.HttpOnly = true;
                    cookieOptions.Secure = true;
                    cookieOptions.SameSite = SameSiteMode.None;
                    cookieOptions.Expires = DateTime.Now.AddDays(-1);
                    cookieOptions.IsEssential = true; // Make the session cookie essential
                }
                HttpContext.Response.Cookies.Append("AuthToken", "", cookieOptions);
                return Ok("Logged out successfully");
            }
            return StatusCode(418, "User is not logged in");

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAccount(Guid userId)
        {
            HttpContext.Request.Cookies.TryGetValue("AuthToken", out var cookie);

            if (string.IsNullOrEmpty(cookie))
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return NoContent();
            else
                return BadRequest(result.Errors);
        }

        [HttpGet]
        public IActionResult user()
        {
            HttpContext.Request.Cookies.TryGetValue("AuthToken", out var cookie);

            if (string.IsNullOrEmpty(cookie))
                return Unauthorized();

            var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(cookie);
            var claims = decodedToken.Claims;
            
            string userId = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
            
            if (string.IsNullOrEmpty(userId))
                userId = User.Claims.FirstOrDefault().Value;

            var user = _userManager.FindByIdAsync(userId);
            if (user == null)
                return Unauthorized();

            return Ok(new
            {
                UserName = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name)?.Value,
                UserId = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value,
                Role = claims.FirstOrDefault(x => x.Type == "role")?.Value, // Check role
                Nome = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.GivenName)?.Value, // Add other claims as needed
                Sobrenome = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.FamilyName)?.Value,
                Email = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)?.Value
            });
        }

        [HttpGet("csrf-token")]
        public IActionResult GetCsrfToken()
        {
            // ASP.NET Core auto-generates and sets the CSRF token in a cookie
            return Ok();
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetSenhaDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Email))
                return BadRequest("Preencha o email");

            if (string.IsNullOrEmpty(dto.NewPassword))
                return BadRequest("Preencha a nova senha");

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(dto.Email);
                if (user == null)
                    return NoContent();
            }
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(dto.Token));
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, dto.NewPassword);
            if (result.Succeeded)
                return Ok("Senha alterada com sucesso");
            return StatusCode(500, result.Errors);
        }

    }
}
