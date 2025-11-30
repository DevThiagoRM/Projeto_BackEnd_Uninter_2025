using System.ComponentModel.DataAnnotations;

namespace SistemaHospitalar_API.Application.Dtos.Consulta
{
    public class EditarConsultaDto
    {
        [Required]
        public Guid PacienteId { get; set; }

        [Required]
        public Guid MedicoId { get; set; }

        [Required]
        public DateTime HorarioConsulta { get; set; }
    }
}
