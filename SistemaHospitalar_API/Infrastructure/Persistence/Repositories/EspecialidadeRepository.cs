using SistemaHospitalar_API.Application.Constructors.Repositories;
using SistemaHospitalar_API.Domain.Entities;

namespace SistemaHospitalar_API.Infrastructure.Persistence.Repositories
{
    public class EspecialidadeRepository : IEspecialidadeRepository
    {
        public Task<Especialidade> CriarEspecialidade(Especialidade especialidade)
        {
            throw new NotImplementedException();
        }

        public Task<Especialidade?> EditarEspecialidade(int id, Especialidade especialidade)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExcluirEspecialidade(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Especialidade?> ObterEspecialidadePorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Especialidade>> ObterEspecialidades()
        {
            throw new NotImplementedException();
        }
    }
}
