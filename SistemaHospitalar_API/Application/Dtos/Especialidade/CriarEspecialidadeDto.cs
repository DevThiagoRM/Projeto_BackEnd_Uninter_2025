using System.ComponentModel.DataAnnotations;

namespace SistemaHospitalar_API.Application.Dtos.Especialidade
{
    public class CriarEspecialidadeDto
    {
        [Required]
        public string Nome { get; set; } = string.Empty;
    }
}
