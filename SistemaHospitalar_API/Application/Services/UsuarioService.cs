using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaHospitalar_API.Application.Constructors.Services;
using SistemaHospitalar_API.Application.Dtos.Usuario;
using SistemaHospitalar_API.Application.Dtos.Medico;
using SistemaHospitalar_API.Application.Dtos.Paciente;
using SistemaHospitalar_API.Domain.Entities;

namespace SistemaHospitalar_API.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ILogger<UsuarioService> _logger;
        private readonly UserManager<Usuario> _userManager;

        public UsuarioService(
            ILogger<UsuarioService> logger,
            UserManager<Usuario> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        // ============================================================================
        // GET ALL
        // ============================================================================
        public async Task<IEnumerable<VisualizarUsuarioDto>> ObterUsuarios()
        {
            _logger.LogInformation("Iniciando consulta de todos usuários.");

            var usuarios = await _userManager.Users
                .AsNoTracking()
                .Include(u => u.Medico)
                    .ThenInclude(m => m.Especialidade)
                .Include(u => u.Paciente)
                .ToListAsync();

            if (!usuarios.Any())
            {
                _logger.LogWarning("Nenhum usuário encontrado.");
                return Enumerable.Empty<VisualizarUsuarioDto>();
            }

            var resultado = usuarios.Select(u => new VisualizarUsuarioDto
            {
                Id = u.Id,
                NomeCompleto = u.NomeCompleto,
                NomeExibicao = u.NomeExibicao,
                DataNascimento = u.DataNascimento,
                Email = u.Email ?? "",
                PhoneNumber = u.PhoneNumber,
                Medico = u.Medico == null ? null : new VisualizarMedicoDto
                {
                    CRM = u.Medico.CRM,
                    Especialidade = u.Medico.Especialidade?.Nome ?? ""
                },
                Paciente = u.Paciente == null ? null : new VisualizarPacienteDto
                {
                    Cpf = u.Paciente.Cpf
                }
            });

            _logger.LogInformation("Consulta concluída. Total retornado: {qtd}", resultado.Count());

            return resultado;
        }

        // ============================================================================
        // GET POR ID
        // ============================================================================
        public async Task<VisualizarUsuarioDto?> ObterUsuarioPorId(Guid id)
        {
            _logger.LogInformation("Consultando usuário pelo ID: {id}", id);

            var usuario = await _userManager.Users
                .AsNoTracking()
                .Include(u => u.Medico)
                    .ThenInclude(m => m.Especialidade)
                .Include(u => u.Paciente)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                _logger.LogWarning("Usuário não encontrado. ID: {id}", id);
                return null;
            }

            return new VisualizarUsuarioDto
            {
                Id = usuario.Id,
                NomeCompleto = usuario.NomeCompleto,
                NomeExibicao = usuario.NomeExibicao,
                DataNascimento = usuario.DataNascimento,
                Email = usuario.Email!,
                PhoneNumber = usuario.PhoneNumber,
                Medico = usuario.Medico == null ? null : new VisualizarMedicoDto
                {
                    CRM = usuario.Medico.CRM,
                    Especialidade = usuario.Medico.Especialidade?.Nome ?? ""
                },
                Paciente = usuario.Paciente == null ? null : new VisualizarPacienteDto
                {
                    Cpf = usuario.Paciente.Cpf
                }
            };
        }

        // ============================================================================
        // GET POR EMAIL
        // ============================================================================
        public async Task<VisualizarUsuarioDto?> ObterUsuarioPorEmail(string email)
        {
            _logger.LogInformation("Consultando usuário por email: {email}", email);

            var usuario = await _userManager.Users
                .AsNoTracking()
                .Include(u => u.Medico)
                .Include(u => u.Paciente)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
            {
                _logger.LogWarning("Nenhum usuário encontrado para o email: {email}", email);
                return null;
            }

            return new VisualizarUsuarioDto
            {
                Id = usuario.Id,
                NomeCompleto = usuario.NomeCompleto,
                NomeExibicao = usuario.NomeExibicao,
                DataNascimento = usuario.DataNascimento,
                Email = usuario.Email!,
                PhoneNumber = usuario.PhoneNumber
            };
        }

        // ============================================================================
        // CREATE
        // ============================================================================
        public async Task<VisualizarUsuarioDto> CriarUsuario(CriarUsuarioDto dto)
        {
            _logger.LogInformation("Iniciando criação de usuário.");

            var emailJaExiste = await _userManager.FindByEmailAsync(dto.Email);

            if (emailJaExiste != null)
            {
                _logger.LogWarning("Erro ao criar: email já está sendo usado: {email}", dto.Email);
                throw new ArgumentException($"Usuário com o email {dto.Email} já existe.");
            }

            var novoUsuario = new Usuario
            {
                NomeCompleto = dto.NomeCompleto,
                NomeExibicao = dto.NomeExibicao,
                DataNascimento = dto.DataNascimento,
                Email = dto.Email,
                UserName = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(novoUsuario, dto.Password);

            if (!result.Succeeded)
            {
                _logger.LogError("Falha ao criar usuário. Erros: {erros}", string.Join(", ", result.Errors.Select(e => e.Description)));
                throw new Exception("Erro ao criar usuário");
            }

            _logger.LogInformation("Usuário criado com sucesso. ID: {id}", novoUsuario.Id);

            return new VisualizarUsuarioDto
            {
                Id = novoUsuario.Id,
                NomeCompleto = novoUsuario.NomeCompleto,
                NomeExibicao = novoUsuario.NomeExibicao,
                DataNascimento = novoUsuario.DataNascimento,
                Email = novoUsuario.Email!,
                PhoneNumber = novoUsuario.PhoneNumber
            };
        }

        // ============================================================================
        // UPDATE
        // ============================================================================
        public async Task<VisualizarUsuarioDto?> EditarUsuario(Guid id, EditarUsuarioDto dto)
        {
            _logger.LogInformation("Iniciando edição do usuário ID: {id}", id);

            var usuarioAtual = await _userManager.FindByIdAsync(id.ToString());

            if (usuarioAtual == null)
            {
                _logger.LogWarning("Usuário não encontrado para edição. ID: {id}", id);
                throw new ArgumentException($"Nenhum usuário encontrado com id: {id}");
            }

            _logger.LogDebug("Atualizando dados do usuário ID: {id}", id);

            usuarioAtual.NomeCompleto = dto.NomeCompleto ?? usuarioAtual.NomeCompleto;
            usuarioAtual.NomeExibicao = dto.NomeExibicao ?? usuarioAtual.NomeExibicao;
            usuarioAtual.DataNascimento = dto.DataNascimento ?? usuarioAtual.DataNascimento;
            usuarioAtual.Email = dto.Email ?? usuarioAtual.Email;
            usuarioAtual.UserName = dto.Email ?? usuarioAtual.UserName;
            usuarioAtual.PhoneNumber = dto.PhoneNumber ?? usuarioAtual.PhoneNumber;

            var result = await _userManager.UpdateAsync(usuarioAtual);

            if (!result.Succeeded)
            {
                _logger.LogError("Erro ao atualizar usuário ID: {id}. Erros: {erros}",
                    id, string.Join(", ", result.Errors.Select(e => e.Description)));
                throw new Exception("Erro ao atualizar usuário.");
            }

            _logger.LogInformation("Usuário atualizado com sucesso. ID: {id}", id);

            return new VisualizarUsuarioDto
            {
                Id = usuarioAtual.Id,
                NomeCompleto = usuarioAtual.NomeCompleto,
                NomeExibicao = usuarioAtual.NomeExibicao,
                DataNascimento = usuarioAtual.DataNascimento,
                Email = usuarioAtual.Email!,
                PhoneNumber = usuarioAtual.PhoneNumber
            };
        }

        // ============================================================================
        // DELETE
        // ============================================================================
        public async Task<bool> ExcluirUsuario(Guid id)
        {
            _logger.LogInformation("Tentando realizar soft delete do usuário ID: {id}", id);

            var usuario = await _userManager.FindByIdAsync(id.ToString());

            if (usuario == null)
            {
                _logger.LogWarning("Soft delete cancelado. Usuário não encontrado. ID: {id}", id);
                return false;
            }

            usuario.Status = false;

            var result = await _userManager.UpdateAsync(usuario);

            if (!result.Succeeded)
            {
                _logger.LogError(
                    "Falha ao realizar soft delete do usuário ID: {id}. Erros: {erros}",
                    id,
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );

                return false;
            }

            _logger.LogInformation("Soft delete realizado com sucesso. ID: {id}", id);

            return true;
        }
    }
}
