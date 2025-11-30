using Microsoft.EntityFrameworkCore;
using SistemaHospitalar_API.Application.Constructors.Repositories;
using SistemaHospitalar_API.Domain.Entities;
using SistemaHospitalar_API.Infrastructure.Data;

namespace SistemaHospitalar_API.Infrastructure.Persistence.Repositories
{
    public class EspecialidadeRepository : IEspecialidadeRepository
    {
        private readonly AppDbContext _context;

        public EspecialidadeRepository(AppDbContext context)
        {
            _context = context;
        }

        // GET
        public async Task<IEnumerable<Especialidade>> ObterEspecialidades()
        {
            return await _context.Especialidades
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Especialidade?> ObterEspecialidadePorId(int id)
        {
            return await _context.Especialidades
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Especialidade?> ObterEspecialidadePorNome(string nome)
        {
            return await _context.Especialidades
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Nome == nome);
        }

        // POST
        public async Task<Especialidade> CriarEspecialidade(Especialidade especialidade)
        {
            await _context.Especialidades.AddAsync(especialidade);
            await _context.SaveChangesAsync();
            return especialidade;
        }

        // PUT
        public async Task<Especialidade?> EditarEspecialidade(int id, Especialidade especialidade)
        {
            var especialidadeAtual = await _context.Especialidades
                .AsTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (especialidadeAtual == null)
                return null;

            especialidadeAtual.Nome = especialidade.Nome;

            await _context.SaveChangesAsync();
            
            return especialidadeAtual;
        }

        // DELETE
        public async Task<bool> ExcluirEspecialidade(int id)
        {
            var especialidade = await _context.Especialidades
                                        .Where(e => e.Id == id)
                                        .ExecuteDeleteAsync();

            return especialidade > 0;
        }
    }
}
