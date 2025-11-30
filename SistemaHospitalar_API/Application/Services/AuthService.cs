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

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
        {
            _logger.LogInformation("Tentativa de login para o email: {Email}", dto.Email);

            // Buscar usuário
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                _logger.LogWarning("Credenciais inválidas para o email: {Email}", dto.Email);
                throw new UnauthorizedAccessException("Credenciais inválidas");
            }

            // Verificar se usuário está ativo
            if (!user.Status)
            {
                _logger.LogWarning("Tentativa de login com usuário inativo: {Email}", dto.Email);
                throw new UnauthorizedAccessException("Usuário inativo");
            }

            // Gerar tokens
            var token = await GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            // Salvar refresh token (simplificado - em produção, salve no banco)
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            // Obter roles do usuário
            var roles = await _userManager.GetRolesAsync(user);

            _logger.LogInformation("Login realizado com sucesso para: {Email}", dto.Email);

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

        public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            _logger.LogInformation("Tentativa de refresh token");

            var principal = GetPrincipalFromExpiredToken(refreshTokenDto.Token);
            var userEmail = principal.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                throw new SecurityTokenException("Token inválido");
            }

            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null || user.RefreshToken != refreshTokenDto.RefreshToken || user.RefreshTokenExpiry <= DateTime.UtcNow)
            {
                throw new SecurityTokenException("Refresh token inválido ou expirado");
            }

            // Gerar novos tokens
            var newToken = await GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            // Atualizar refresh token
            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

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

        public async Task<bool> RevokeTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            return true;
        }

        private async Task<string> GenerateJwtToken(Usuario user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.NomeCompleto),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Adicionar roles como claims
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));
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
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Token inválido");

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
