using System.ComponentModel.DataAnnotations;

namespace SistemaHospitalar_API.Application.Dtos.Medico
{
    public class CriarMedicoDto
    {
        [Required]
        public string CRM { get; set; } = string.Empty;

        [Required]
        public int EspecialidadeId { get; set; }
    }
}
