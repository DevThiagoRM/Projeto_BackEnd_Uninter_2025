// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SistemaHospitalar_API.Application.Constructors.Services;
using SistemaHospitalar_API.Application.Dtos.Auth;

namespace SistemaHospitalar_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _authService.LoginAsync(loginDto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Falha no login para {Email}: {Message}", loginDto.Email, ex.Message);
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante o login para {Email}", loginDto.Email);
                return StatusCode(500, new { message = "Erro interno durante o login" });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _authService.RefreshTokenAsync(refreshTokenDto);
                return Ok(result);
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogWarning("Refresh token inválido: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante refresh token");
                return StatusCode(500, new { message = "Erro interno durante refresh token" });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string email)
        {
            try
            {
                await _authService.RevokeTokenAsync(email);
                return Ok(new { message = "Logout realizado com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante logout para {Email}", email);
                return StatusCode(500, new { message = "Erro interno durante logout" });
            }
        }
    }
}