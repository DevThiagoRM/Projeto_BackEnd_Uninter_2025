using SistemaHospitalar_API.Application.Constructors.Repositories;
using SistemaHospitalar_API.Application.Constructors.Services;
using SistemaHospitalar_API.Application.Dtos.Medico;
using SistemaHospitalar_API.Domain.Entities;

namespace SistemaHospitalar_API.Application.Services
{
    public class MedicoService : IMedicoService
    {
        private readonly ILogger<MedicoService> _logger;
        private readonly IMedicoRepository _repo;

        public MedicoService(
            ILogger<MedicoService> logger,
            IMedicoRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public async Task<VisualizarMedicoDto> CriarMedico(Guid id, CriarMedicoDto dto)
        {
            // Cria a entidade a partir do DTO
            var medico = new Medico
            {
                Id = id,
                CRM = dto.CRM,
                EspecialidadeId = dto.EspecialidadeId,
            };

            // Persiste no banco
            var medicoCriado = await _repo.CriarMedico(medico);

            // Retorna DTO de visualização
            return new VisualizarMedicoDto
            {
                CRM = medicoCriado.CRM,
                Especialidade = medicoCriado.Especialidade!.Nome,
            };
        }

        public async Task<VisualizarMedicoDto?> EditarMedico(Guid id, EditarMedicoDto dto)
        {
            // Cria uma entidade parcial para atualização
            var medicoParaAtualizar = new Medico
            {
                CRM = dto.CRM,
                EspecialidadeId = dto.EspecialidadeId
            };

            var medicoAtualizado = await _repo.EditarMedico(id, medicoParaAtualizar);

            if (medicoAtualizado == null)
                return null;

            return new VisualizarMedicoDto
            {
                CRM = medicoAtualizado.CRM,
                Especialidade = medicoAtualizado.Especialidade!.Nome,
            };
        }

        public async Task<bool> ExcluirMedico(Guid id)
        {
            return await _repo.ExcluirMedico(id);
        }
    }
}
