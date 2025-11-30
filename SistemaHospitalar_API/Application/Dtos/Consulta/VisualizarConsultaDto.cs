namespace SistemaHospitalar_API.Application.Dtos.Consulta
{
    public class VisualizarConsultaDto
    {
        public Guid Id { get; set; }
        public string NomePaciente { get; set; } = string.Empty;
        public string NomeMedico { get; set; } = string.Empty;
        public string EspecialidadeMedico { get; set; } = string.Empty;
        public DateTime HorarioConsulta { get; set; }
        public string? Observacao { get; set; }
        public bool Status { get; set; }
    }
}
