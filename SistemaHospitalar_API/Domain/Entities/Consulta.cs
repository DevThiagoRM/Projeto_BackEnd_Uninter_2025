namespace SistemaHospitalar_API.Domain.Entities
{
    public class Consulta
    {
        // Dados Consulta
        public Guid Id { get; set; }

        public Usuario Usuario { get; set; }
        public Guid PacienteId { get; set; }

        public Medico Medico { get; set; }
        public Guid MedicoId { get; set; }

        public DateTime HorarioConsulta { get; set; }
    }
}
