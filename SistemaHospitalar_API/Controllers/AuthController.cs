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

        // ========================================================================
        // LOGIN
        // ========================================================================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Dados inválidos ao tentar login: {@ModelState}", ModelState);
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Tentativa de login para {Email}", loginDto.Email);
                var result = await _authService.LoginAsync(loginDto);
                _logger.LogInformation("Login realizado com sucesso para {Email}", loginDto.Email);

                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Falha no login para {Email}", loginDto.Email);
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno durante login para {Email}", loginDto.Email);
                return StatusCode(500, new { message = "Erro interno durante o login" });
            }
        }

        // ========================================================================
        // REFRESH TOKEN
        // ========================================================================
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Dados inválidos ao tentar refresh token: {@ModelState}", ModelState);
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Solicitação de refresh token para token: {Token}", refreshTokenDto.RefreshToken);
                var result = await _authService.RefreshTokenAsync(refreshTokenDto);
                _logger.LogInformation("Refresh token realizado com sucesso para token: {Token}", refreshTokenDto.RefreshToken);

                return Ok(result);
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogWarning(ex, "Refresh token inválido para token: {Token}", refreshTokenDto.RefreshToken);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno durante refresh token para token: {Token}", refreshTokenDto.RefreshToken);
                return StatusCode(500, new { message = "Erro interno durante refresh token" });
            }
        }

        // ========================================================================
        // LOGOUT
        // ========================================================================
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string email)
        {
            try
            {
                _logger.LogInformation("Tentativa de logout para {Email}", email);
                await _authService.RevokeTokenAsync(email);
                _logger.LogInformation("Logout realizado com sucesso para {Email}", email);

                return Ok(new { message = "Logout realizado com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno durante logout para {Email}", email);
                return StatusCode(500, new { message = "Erro interno durante logout" });
            }
        }
    }
}
