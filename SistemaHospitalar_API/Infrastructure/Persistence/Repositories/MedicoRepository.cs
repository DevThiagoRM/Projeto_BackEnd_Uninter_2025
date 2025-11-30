using SistemaHospitalar_API.Application.Constructors.Repositories;

namespace SistemaHospitalar_API.Infrastructure.Persistence.Repositories
{
    public class MedicoRepository : IMedicoRepository
    {
        public Task<Medico> CriarMedico(Medico medico)
        {
            throw new NotImplementedException();
        }

        public Task<Medico?> EditarMedico(Guid id, Medico medico)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExcluirMedico(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Medico?> ObterMedicoPorId(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Medico>> ObterMedicos()
        {
            throw new NotImplementedException();
        }
    }
}
