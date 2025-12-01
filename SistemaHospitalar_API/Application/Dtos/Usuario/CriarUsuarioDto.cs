using SistemaHospitalar_API.Application.Dtos.Medico;
using SistemaHospitalar_API.Application.Dtos.Paciente;
using System.ComponentModel.DataAnnotations;

namespace SistemaHospitalar_API.Application.Dtos.Usuario
{
    public class CriarUsuarioDto
    {
        // Dados Usuário
        [Required]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required]
        public string NomeExibicao { get; set; } = string.Empty;

        public DateTime? DataNascimento { get; set; }
        public bool AlterarSenhaNoProximoLogin { get; set; } = true;

        // Dados Identity
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public CriarMedicoDto? Medico { get; set; }
        public CriarPacienteDto? Paciente { get; set; }

        public bool IsMedico { get; set; } = false;
        public bool IsPaciente { get; set; } = false;
        public bool Status { get; set; }
    }
}
