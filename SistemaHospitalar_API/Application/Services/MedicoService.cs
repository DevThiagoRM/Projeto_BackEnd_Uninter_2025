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

        // ======================
        // CREATE
        // ======================
        public async Task<VisualizarMedicoDto> CriarMedico(Guid id, CriarMedicoDto dto)
        {
            _logger.LogInformation("Iniciando criação de médico para usuário ID: {id}, CRM: {crm}", id, dto.CRM);

            var medico = new Medico
            {
                Id = id,
                CRM = dto.CRM,
                EspecialidadeId = dto.EspecialidadeId
            };

            var medicoCriado = await _repo.CriarMedico(medico);

            _logger.LogInformation("Médico criado com sucesso para usuário ID: {id}, CRM: {crm}", id, medicoCriado.CRM);

            return new VisualizarMedicoDto
            {
                CRM = medicoCriado.CRM,
                Especialidade = medicoCriado.Especialidade!.Nome
            };
        }

        // ======================
        // EDIT
        // ======================
        public async Task<VisualizarMedicoDto?> EditarMedico(Guid id, EditarMedicoDto dto)
        {
            _logger.LogInformation("Iniciando edição de médico para usuário ID: {id}", id);

            _logger.LogDebug("Novos valores recebidos: CRM={crm}, EspecialidadeId={especialidadeId}", dto.CRM, dto.EspecialidadeId);

            var medicoParaAtualizar = new Medico
            {
                CRM = dto.CRM,
                EspecialidadeId = dto.EspecialidadeId
            };

            var medicoAtualizado = await _repo.EditarMedico(id, medicoParaAtualizar);

            if (medicoAtualizado == null)
            {
                _logger.LogWarning("Falha ao atualizar médico. Usuário ID não encontrado: {id}", id);
                return null;
            }

            _logger.LogInformation("Médico atualizado com sucesso para usuário ID: {id}, CRM: {crm}", id, medicoAtualizado.CRM);

            return new VisualizarMedicoDto
            {
                CRM = medicoAtualizado.CRM,
                Especialidade = medicoAtualizado.Especialidade!.Nome
            };
        }

        // ======================
        // DELETE
        // ======================
        public async Task<bool> ExcluirMedico(Guid id)
        {
            _logger.LogInformation("Iniciando exclusão de médico para usuário ID: {id}", id);

            var excluiu = await _repo.ExcluirMedico(id);

            if (!excluiu)
            {
                _logger.LogWarning("Falha ao excluir médico. Usuário ID: {id} não encontrado ou já excluído", id);
                return false;
            }

            _logger.LogInformation("Médico excluído com sucesso para usuário ID: {id}", id);
            return true;
        }
    }
}
