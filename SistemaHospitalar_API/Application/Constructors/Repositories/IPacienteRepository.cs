namespace SistemaHospitalar_API.Application.Constructors.Repositories
{
    public interface IPacienteRepository
    {
        Task<Paciente> CriarPaciente(Paciente paciente);
        Task<Paciente?> EditarPaciente(Guid id, Paciente paciente);
        Task<bool> ExcluirPaciente(Guid id);
    }
}
