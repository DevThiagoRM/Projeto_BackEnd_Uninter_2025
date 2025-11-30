using Microsoft.AspNetCore.Identity;

namespace SistemaHospitalar_API.Domain.Entities
{
    public class Usuario : IdentityUser<Guid>
    {
        // Dados Usuário
        public string NomeCompleto { get; set; } = string.Empty;
        public string NomeExibicao { get; set; } = string.Empty;
        public DateTime? DataNascimento { get; set; }
        public bool AlterarSenhaNoProximoLogin { get; set; } = true;

        // Navegações
        public Medico? Medico { get; set; }
        public Paciente? Paciente { get; set; }

        // Refresh Token
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        public bool IsMedico { get; set; } = false;
        public bool IsPaciente { get; set; } = false;
        public bool Status { get; set; } = true;
        

    }
}
