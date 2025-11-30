using SistemaHospitalar_API.Application.Dtos.Auth;

namespace SistemaHospitalar_API.Application.Constructors.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
        Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenDto dto);
        Task<bool> RevokeTokenAsync(string email);
    }
}
