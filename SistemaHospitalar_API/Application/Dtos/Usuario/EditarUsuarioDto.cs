using SistemaHospitalar_API.Application.Dtos.Medico;
using SistemaHospitalar_API.Application.Dtos.Paciente;

namespace SistemaHospitalar_API.Application.Dtos.Usuario
{
    public class EditarUsuarioDto
    {
        // Dados Usuário
        public string NomeCompleto { get; set; } = string.Empty;
        public string NomeExibicao { get; set; } = string.Empty;
        public DateTime? DataNascimento { get; set; }
        public bool AlterarSenhaNoProximoLogin { get; set; } = true;

        // Dados Identity
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }

        public bool Status { get; set; }
    }
}
