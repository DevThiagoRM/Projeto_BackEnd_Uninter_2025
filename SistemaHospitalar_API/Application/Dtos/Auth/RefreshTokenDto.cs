using System.ComponentModel.DataAnnotations;

namespace SistemaHospitalar_API.Application.Dtos.Auth
{
    public class RefreshTokenDto
    {
        [Required(ErrorMessage = "Token é obrigatório")]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "Refresh token é obrigatório")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
