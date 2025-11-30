using SistemaHospitalar_API.Application.Dtos.Medico;
using SistemaHospitalar_API.Application.Dtos.Paciente;

namespace SistemaHospitalar_API.Application.Dtos.Usuario
{
    public class VisualizarUsuarioDto
    {
        public Guid Id { get; set; }
        public string NomeCompleto { get; set; } = string.Empty;
        public string NomeExibicao { get; set; } = string.Empty;
        public DateTime? DataNascimento { get; set; }

        // Dados Identity
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }

        public VisualizarMedicoDto? Medico { get; set; }
        public VisualizarPacienteDto? Paciente { get; set; }
    }
}
