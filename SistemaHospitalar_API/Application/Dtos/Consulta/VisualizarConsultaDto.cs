namespace SistemaHospitalar_API.Application.Dtos.Consulta
{
    public class VisualizarConsultaDto
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public Guid MedicoId { get; set; }
        public DateTime HorarioConsulta { get; set; }
    }
}
