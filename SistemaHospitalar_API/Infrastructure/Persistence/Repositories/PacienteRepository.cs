using Microsoft.EntityFrameworkCore;
using SistemaHospitalar_API.Application.Constructors.Repositories;
using SistemaHospitalar_API.Infrastructure.Data;

namespace SistemaHospitalar_API.Infrastructure.Persistence.Repositories
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly AppDbContext _context;

        public PacienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Paciente> CriarPaciente(Paciente paciente)
        {
            await _context.Pacientes.AddAsync(paciente);
            await _context.SaveChangesAsync();
            return paciente;
        }

        public async Task<Paciente?> EditarPaciente(Guid id, Paciente paciente)
        {
            var pacienteAtual = await _context.Pacientes
                .AsTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (pacienteAtual == null)
                return null;

            pacienteAtual.Cpf = paciente.Cpf ?? pacienteAtual.Cpf;

            await _context.SaveChangesAsync();
            return pacienteAtual;
        }

        public async Task<bool> ExcluirPaciente(Guid id)
        {
            var paciente = await _context.Pacientes.FirstOrDefaultAsync(m => m.Id == id);

            if (paciente == null)
                return false;

            paciente.Status = false;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
