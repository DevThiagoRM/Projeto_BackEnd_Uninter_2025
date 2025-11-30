using System.ComponentModel.DataAnnotations;

namespace SistemaHospitalar_API.Application.Dtos.Paciente
{
    public class CriarPacienteDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Cpf { get; set; } = string.Empty;
    }
}
