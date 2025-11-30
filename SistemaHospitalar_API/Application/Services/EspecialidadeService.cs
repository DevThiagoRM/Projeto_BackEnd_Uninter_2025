using SistemaHospitalar_API.Application.Constructors.Repositories;
using SistemaHospitalar_API.Application.Constructors.Services;
using SistemaHospitalar_API.Application.Dtos.Especialidade;
using SistemaHospitalar_API.Domain.Entities;

namespace SistemaHospitalar_API.Application.Services
{
    public class EspecialidadeService : IEspecialidadeService
    {
        private readonly ILogger<EspecialidadeService> _logger;
        private readonly IEspecialidadeRepository _repo;

        public EspecialidadeService(
            ILogger<EspecialidadeService> logger,
            IEspecialidadeRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        // ============================================================================
        // GET ALL
        // ============================================================================
        public async Task<IEnumerable<VisualizarEspecialidadeDto>> ObterEspecialidades()
        {
            _logger.LogInformation("Iniciando consulta de todas as especialidades.");

            var especialidades = await _repo.ObterEspecialidades();

            if (!especialidades.Any())
            {
                _logger.LogWarning("Nenhuma especialidade encontrada no banco.");
                return new List<VisualizarEspecialidadeDto>();
            }

            var resultado = especialidades.Select(e => new VisualizarEspecialidadeDto
            {
                Id = e.Id,
                Nome = e.Nome
            });

            _logger.LogInformation("Consulta concluída. Total de especialidades retornadas: {count}", resultado.Count());

            return resultado;
        }

        // ============================================================================
        // GET BY ID
        // ============================================================================
        public async Task<VisualizarEspecialidadeDto?> ObterEspecialidadePorId(int id)
        {
            _logger.LogInformation("Consultando especialidade com id: {id}", id);

            var especialidade = await _repo.ObterEspecialidadePorId(id);

            if (especialidade == null)
            {
                _logger.LogWarning("Especialidade não encontrada para o id: {id}", id);
                return null;
            }

            return new VisualizarEspecialidadeDto
            {
                Id = especialidade.Id,
                Nome = especialidade.Nome
            };
        }

        // ============================================================================
        // POST
        // ============================================================================
        public async Task<VisualizarEspecialidadeDto> CriarEspecialidade(CriarEspecialidadeDto dto)
        {
            _logger.LogInformation("Iniciando criação de especialidade. Nome: {nome}", dto.Nome);

            var especialidadeExistente = await _repo.ObterEspecialidadePorNome(dto.Nome);

            if (especialidadeExistente != null)
            {
                _logger.LogWarning("Erro ao criar: Especialidade '{nome}' já existe.", dto.Nome);
                throw new ArgumentException("Especialidade já existe.");
            }

            var novaEspecialidade = new Especialidade
            {
                Nome = dto.Nome
            };

            var result = await _repo.CriarEspecialidade(novaEspecialidade);

            _logger.LogInformation("Especialidade criada com sucesso. ID: {id}", result.Id);

            return new VisualizarEspecialidadeDto
            {
                Id = result.Id,
                Nome = result.Nome
            };
        }

        // ============================================================================
        // PUT
        // ============================================================================
        public async Task<VisualizarEspecialidadeDto?> EditarEspecialidade(int id, EditarEspecialidadeDto dto)
        {
            _logger.LogInformation("Iniciando atualização da especialidade com ID: {id}", id);

            var especialidadeAtual = await _repo.ObterEspecialidadePorId(id);

            if (especialidadeAtual == null)
            {
                _logger.LogWarning("Especialidade não encontrada para atualização. ID: {id}", id);
                throw new ArgumentException($"Nenhuma especialidade encontrada com id: {id}");
            }

            _logger.LogDebug("Valor atual do nome: {atual}. Novo valor: {novo}", especialidadeAtual.Nome, dto.Nome);

            especialidadeAtual.Nome = dto.Nome ?? especialidadeAtual.Nome;

            var result = await _repo.EditarEspecialidade(id, especialidadeAtual);

            _logger.LogInformation("Especialidade atualizada com sucesso. ID: {id}", id);

            return new VisualizarEspecialidadeDto
            {
                Id = result!.Id,
                Nome = result.Nome
            };
        }

        // ============================================================================
        // DELETE
        // ============================================================================
        public async Task<bool> ExcluirEspecialidade(int id)
        {
            _logger.LogInformation("Tentando excluir especialidade com ID: {id}", id);

            var excluiu = await _repo.ExcluirEspecialidade(id);

            if (!excluiu)
            {
                _logger.LogWarning("Nenhuma especialidade excluída. ID informado: {id}", id);
                return false;
            }

            _logger.LogInformation("Especialidade excluída com sucesso. ID: {id}", id);
            return true;
        }
    }
}
