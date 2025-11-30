using System.ComponentModel.DataAnnotations;

namespace SistemaHospitalar_API.Application.Dtos.Medico
{
    public class VisualizarMedicoDto
    {
        public string CRM { get; set; } = string.Empty;
        public string Especialidade { get; set; } = string.Empty;
    }
}
