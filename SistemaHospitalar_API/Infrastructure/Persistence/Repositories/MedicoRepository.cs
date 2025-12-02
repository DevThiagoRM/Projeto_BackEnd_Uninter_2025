using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaHospitalar_API.Application.Constructors.Repositories;
using SistemaHospitalar_API.Domain.Entities;
using SistemaHospitalar_API.Infrastructure.Data;

namespace SistemaHospitalar_API.Infrastructure.Persistence.Repositories
{
    public class MedicoRepository : IMedicoRepository
    {
        private readonly AppDbContext _context;

        public MedicoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Medico> CriarMedico(Medico medico)
        {
            await _context.Medicos.AddAsync(medico);
            await _context.SaveChangesAsync();

            var medicoComEspecialidade = await _context.Medicos
                                                .Include(m => m.Especialidade)
                                                .FirstOrDefaultAsync(m => m.Id == medico.Id);
            return medicoComEspecialidade!;
        }

        public async Task<Medico?> EditarMedico(Guid id, Medico medico)
        {
            var medicoAtual = await _context.Medicos
                .AsTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (medicoAtual == null)
                return null;

            medicoAtual.CRM = medico.CRM ?? medicoAtual.CRM;
            medicoAtual.EspecialidadeId = medico.EspecialidadeId;

            await _context.SaveChangesAsync();
            return medicoAtual;
        }

        public async Task<bool> ExcluirMedico(Guid id)
        {
            var medico = await _context.Medicos.FirstOrDefaultAsync(m => m.Id == id);

            if (medico == null)
                return false;

            medico.Status = false;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Medico> ObterMedicoPorCRM(string crm)
        {
            return await _context.Medicos
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.CRM == crm);
        }
    }
}
