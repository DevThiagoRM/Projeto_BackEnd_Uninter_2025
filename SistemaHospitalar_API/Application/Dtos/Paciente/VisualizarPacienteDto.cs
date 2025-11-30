using System.ComponentModel.DataAnnotations;

namespace SistemaHospitalar_API.Application.Dtos.Paciente
{
    public class VisualizarPacienteDto
    {
        public string Cpf { get; set; } = string.Empty;
    }
}
