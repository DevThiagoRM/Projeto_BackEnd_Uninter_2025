using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SistemaHospitalar_API.Application.Constructors.Services;
using SistemaHospitalar_API.Application.Dtos.Auth;
using SistemaHospitalar_API.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SistemaHospitalar_API.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<Usuario> userManager,
            IConfiguration configuration,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        // ============================================================
        // LOGIN
        // ============================================================
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
        {
            _logger.LogInformation("Tentativa de login para o email: {Email}", dto.Email);

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                _logger.LogWarning("Credenciais inválidas para {Email}", dto.Email);
                throw new UnauthorizedAccessException("Credenciais inválidas.");
            }

            if (!user.Status)
            {
                _logger.LogWarning("Tentativa de login com usuário inativo: {Email}", dto.Email);
                throw new UnauthorizedAccessException("Usuário inativo.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var token = await GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            _logger.LogInformation("Login bem-sucedido para {Email}", dto.Email);

            return new LoginResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(GetTokenExpirationMinutes()),
                NomeUsuario = user.NomeCompleto,
                Email = user.Email!,
                Roles = roles
            };
        }

        // ============================================================
        // REFRESH TOKEN
        // ============================================================
        public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenDto dto)
        {
            _logger.LogInformation("Tentativa de refresh token iniciada.");

            var principal = GetPrincipalFromExpiredToken(dto.Token);
            var email = principal.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
                throw new SecurityTokenException("Token inválido.");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null ||
                user.RefreshToken != dto.RefreshToken ||
                user.RefreshTokenExpiry <= DateTime.UtcNow)
            {
                _logger.LogWarning("Refresh token inválido ou expirado para {Email}", email);
                throw new SecurityTokenException("Refresh token inválido ou expirado.");
            }

            var newToken = await GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            _logger.LogInformation("Refresh token renovado para {Email}", email);

            return new LoginResponseDto
            {
                Token = newToken,
                RefreshToken = newRefreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(GetTokenExpirationMinutes()),
                NomeUsuario = user.NomeCompleto,
                Email = user.Email!,
                Roles = roles
            };
        }

        // ============================================================
        // REVOGAR TOKEN
        // ============================================================
        public async Task<bool> RevokeTokenAsync(string email)
        {
            _logger.LogInformation("Tentativa de revogação do refresh token para {Email}", email);

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Usuário não encontrado para revogação: {Email}", email);
                return false;
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;

            await _userManager.UpdateAsync(user);

            _logger.LogInformation("Refresh token revogado para {Email}", email);

            return true;
        }

        // ============================================================
        // MÉTODOS INTERNOS (AUXILIARES)
        // ============================================================
        private async Task<string> GenerateJwtToken(Usuario user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.NomeCompleto),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(GetTokenExpirationMinutes()),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var bytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var parameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!)
                )
            };

            var handler = new JwtSecurityTokenHandler();

            var principal = handler.ValidateToken(token, parameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwt ||
                !jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
            {
                throw new SecurityTokenException("Token inválido.");
            }

            return principal;
        }

        private int GetTokenExpirationMinutes()
        {
            return int.TryParse(_configuration["JwtSettings:ExpirationInMinutes"], out var minutes)
                ? minutes
                : 60;
        }
    }
}
