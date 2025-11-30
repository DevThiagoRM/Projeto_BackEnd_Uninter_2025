namespace SistemaHospitalar_API.Application.Constructors.Repositories
{
    public interface IMedicoRepository
    {
        Task<IEnumerable<Medico>> ObterMedicos();
        Task<Medico?> ObterMedicoPorId(Guid id);
        Task<Medico> CriarMedico(Medico medico);
        Task<Medico?> EditarMedico(Guid id, Medico medico);
        Task<bool> ExcluirMedico(Guid id);

    }
}
