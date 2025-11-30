using Microsoft.EntityFrameworkCore;
using SistemaHospitalar_API.Application.Constructors.Repositories;
using SistemaHospitalar_API.Domain.Entities;
using SistemaHospitalar_API.Infrastructure.Data;

namespace SistemaHospitalar_API.Infrastructure.Persistence.Repositories
{
    public class ConsultaRepository : IConsultaRepository
    {
        private readonly AppDbContext _context;

        public ConsultaRepository(AppDbContext context)
        {
            _context = context;
        }

        // ============================================================================
        // GET ALL
        // ============================================================================
        public async Task<IEnumerable<Consulta>> ObterConsultas()
        {
            return await _context.Consultas
                .Include(c => c.Paciente)
                    .ThenInclude(p => p!.Usuario)
                .Include(c => c.Medico)
                    .ThenInclude(m => m!.Usuario)
                .AsNoTracking()
                .ToListAsync();
        }

        // ============================================================================
        // GET BY ID
        // ============================================================================
        public async Task<Consulta?> ObterConsultasPorId(Guid id)
        {
            return await _context.Consultas
                .Include(c => c.Paciente)
                    .ThenInclude(p => p!.Usuario)
                .Include(c => c.Medico)
                    .ThenInclude(m => m!.Usuario)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // ============================================================================
        // GET BY MEDICO ID
        // ============================================================================
        public async Task<IEnumerable<Consulta>> ObterConsultasPorMedicoId(Guid medicoId)
        {
            return await _context.Consultas
                .Include(c => c.Paciente)
                    .ThenInclude(p => p!.Usuario)
                .Include(c => c.Medico)
                    .ThenInclude(m => m!.Usuario)
                .Where(c => c.MedicoId == medicoId)
                .AsNoTracking()
                .ToListAsync();
        }

        // ============================================================================
        // GET BY PACIENTE ID
        // ============================================================================
        public async Task<IEnumerable<Consulta>> ObterConsultasPorPacienteId(Guid pacienteId)
        {
            return await _context.Consultas
                .Include(c => c.Paciente)
                    .ThenInclude(p => p!.Usuario)
                .Include(c => c.Medico)
                    .ThenInclude(m => m!.Usuario)
                .Where(c => c.PacienteId == pacienteId)
                .AsNoTracking()
                .ToListAsync();
        }

        // ============================================================================
        // GET BY MEDICO NOME
        // ============================================================================
        public async Task<IEnumerable<Consulta>> ObterConsultasPorMedicoNome(string medicoNome)
        {
            return await _context.Consultas
                .Include(c => c.Paciente)
                    .ThenInclude(p => p!.Usuario)
                .Include(c => c.Medico)
                    .ThenInclude(m => m!.Usuario)
                .Where(c => c.Medico != null && c.Medico.Usuario != null &&
                            EF.Functions.Like(c.Medico.Usuario.NomeCompleto, $"%{medicoNome}%"))
                .AsNoTracking()
                .ToListAsync();
        }

        // ============================================================================
        // GET BY PACIENTE NOME
        // ============================================================================
        public async Task<IEnumerable<Consulta>> ObterConsultasPorPacienteNome(string pacienteNome)
        {
            return await _context.Consultas
                .Include(c => c.Paciente)
                    .ThenInclude(p => p!.Usuario)
                .Include(c => c.Medico)
                    .ThenInclude(m => m!.Usuario)
                .Where(c => c.Paciente != null && c.Paciente.Usuario != null &&
                            EF.Functions.Like(c.Paciente.Usuario.NomeCompleto, $"%{pacienteNome}%"))
                .AsNoTracking()
                .ToListAsync();
        }

        // ============================================================================
        // GET BY PERÍODO
        // ============================================================================

        public async Task<IEnumerable<Consulta>> ObterConsultasPorPeriodo(DateTime? dataInicial, DateTime? dataFinal)
        {
            var query = _context.Consultas
                                .AsNoTracking()
                                .Include(c => c.Medico)
                                    .ThenInclude(m => m!.Usuario)
                                .Include(c => c.Paciente)
                                    .ThenInclude(p => p!.Usuario)
                                .AsQueryable();

            if (dataInicial.HasValue)
            {
                query = query.Where(c => c.HorarioConsulta >= dataInicial.Value);
            }

            if (dataFinal.HasValue)
            {
                query = query.Where(c => c.HorarioConsulta <= dataFinal.Value);
            }

            query = query.OrderBy(c => c.HorarioConsulta);

            return await query.ToListAsync();
        }

        // ============================================================================
        // POST
        // ============================================================================
        public async Task<Consulta> CriarConsulta(Consulta consulta)
        {
            await _context.Consultas.AddAsync(consulta);
            await _context.SaveChangesAsync();
            return consulta;
        }

        // ============================================================================
        // PUT
        // ============================================================================
        public async Task<Consulta?> EditarConsulta(Guid id, Consulta consulta)
        {
            var consultaAtual = await _context.Consultas
                .AsTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (consultaAtual == null)
                return null;

            consultaAtual.PacienteId = consulta.PacienteId;
            consultaAtual.MedicoId = consulta.MedicoId;
            consultaAtual.HorarioConsulta = consulta.HorarioConsulta;
            consultaAtual.Observacao = consulta.Observacao;
            consultaAtual.Status = consulta.Status;

            await _context.SaveChangesAsync();
            return consultaAtual;
        }

        // ============================================================================
        // CANCELAR CONSULTA (DELETE LÓGICO)
        // ============================================================================
        public async Task<bool> CancelarConsulta(Guid id, string motivo)
        {
            var consulta = await _context.Consultas
                .AsTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (consulta == null)
                return false;

            // Salvar o motivo no campo Observacao e desativar
            consulta.Observacao = motivo;
            consulta.Status = false;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
