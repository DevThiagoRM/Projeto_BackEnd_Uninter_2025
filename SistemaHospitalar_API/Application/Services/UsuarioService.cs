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
        private readonly IMedicoService _medicoService;
        private readonly IPacienteService _pacienteService;
        private readonly IEspecialidadeService _especialidadeService;

        public UsuarioService(
            ILogger<UsuarioService> logger,
            UserManager<Usuario> userManager,
            IMedicoService medicoService,
            IPacienteService pacienteService,
            IEspecialidadeService especialidadeService)
        {
            _logger = logger;
            _userManager = userManager;
            _medicoService = medicoService;
            _pacienteService = pacienteService;
            _especialidadeService = especialidadeService;
        }

        // ======================
        // GET ALL
        // ======================
        public async Task<IEnumerable<VisualizarUsuarioDto>> ObterUsuarios()
        {
            _logger.LogInformation("Iniciando consulta de todos os usuários.");

            var usuarios = await _userManager.Users
                .AsNoTracking()
                .Include(u => u.Medico)
                    .ThenInclude(m => m!.Especialidade)
                .Include(u => u.Paciente)
                .ToListAsync();

            if (!usuarios.Any())
            {
                _logger.LogWarning("Nenhum usuário encontrado no banco.");
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
                },
                IsMedico = u.Medico != null,
                IsPaciente = u.Paciente != null,
                Status = u.Status != null
            });

            _logger.LogInformation("Consulta concluída. Total de usuários retornados: {count}", resultado.Count());

            return resultado;
        }

        // ======================
        // GET POR ID
        // ======================
        public async Task<VisualizarUsuarioDto?> ObterUsuarioPorId(Guid id)
        {
            _logger.LogInformation("Consultando usuário pelo ID: {id}", id);

            var usuario = await _userManager.Users
                .AsNoTracking()
                .Include(u => u.Medico)
                    .ThenInclude(m => m!.Especialidade)
                .Include(u => u.Paciente)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                _logger.LogWarning("Usuário não encontrado. ID: {id}", id);
                return null;
            }

            _logger.LogInformation("Usuário encontrado. ID: {id}, Email: {email}", usuario.Id, usuario.Email);

            return new VisualizarUsuarioDto
            {
                Id = usuario.Id,
                NomeCompleto = usuario.NomeCompleto,
                NomeExibicao = usuario.NomeExibicao,
                DataNascimento = usuario.DataNascimento,
                Email = usuario.Email ?? "",
                PhoneNumber = usuario.PhoneNumber,
                Medico = usuario.Medico == null ? null : new VisualizarMedicoDto
                {
                    CRM = usuario.Medico.CRM,
                    Especialidade = usuario.Medico.Especialidade?.Nome ?? ""
                },
                Paciente = usuario.Paciente == null ? null : new VisualizarPacienteDto
                {
                    Cpf = usuario.Paciente.Cpf
                },
                IsMedico = usuario.Medico != null,
                IsPaciente = usuario.Paciente != null,
                Status = usuario.Status != null
            };
        }

        // ======================
        // GET POR EMAIL
        // ======================
        public async Task<VisualizarUsuarioDto?> ObterUsuarioPorEmail(string email)
        {
            _logger.LogInformation("Consultando usuário pelo email: {email}", email);

            var usuario = await _userManager.Users
                .AsNoTracking()
                .Include(u => u.Medico)
                    .ThenInclude(m => m!.Especialidade)
                .Include(u => u.Paciente)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
            {
                _logger.LogWarning("Nenhum usuário encontrado para o email: {email}", email);
                return null;
            }

            _logger.LogInformation("Usuário encontrado. ID: {id}, Email: {email}", usuario.Id, usuario.Email);

            return new VisualizarUsuarioDto
            {
                Id = usuario.Id,
                NomeCompleto = usuario.NomeCompleto,
                NomeExibicao = usuario.NomeExibicao,
                DataNascimento = usuario.DataNascimento,
                Email = usuario.Email ?? "",
                PhoneNumber = usuario.PhoneNumber,
                Medico = usuario.Medico == null ? null : new VisualizarMedicoDto
                {
                    CRM = usuario.Medico.CRM,
                    Especialidade = usuario.Medico.Especialidade?.Nome ?? ""
                },
                Paciente = usuario.Paciente == null ? null : new VisualizarPacienteDto
                {
                    Cpf = usuario.Paciente.Cpf
                },
                IsMedico = usuario.Medico != null,
                IsPaciente = usuario.Paciente != null,
                Status = usuario.Status != null
            };
        }

        // ======================
        // CREATE
        // ======================
        public async Task<VisualizarUsuarioDto> CriarUsuario(CriarUsuarioDto dto)
        {
            _logger.LogInformation("Iniciando criação de usuário. Email: {email}", dto.Email);

            // Validação de email único
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
            {
                _logger.LogWarning("Erro ao criar: email já está em uso: {email}", dto.Email);
                throw new ArgumentException($"Usuário com o email {dto.Email} já existe.");
            }

            // Validação de CPF único para paciente
            if (dto.IsPaciente && dto.Paciente != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Paciente.Cpf))
                {
                    _logger.LogWarning("CPF do paciente não informado.");
                    throw new ArgumentException("CPF é obrigatório.");
                }
                if (!ValidarCPF(dto.Paciente.Cpf))
                {
                    _logger.LogWarning("CPF inválido informado: {cpf}", dto.Paciente.Cpf);
                    throw new ArgumentException("CPF inválido.");
                }

                // Verificar se já existe um paciente com o mesmo CPF
                var pacienteExistente = await _pacienteService.ObterPacientePorCpf(dto.Paciente.Cpf);
                if (pacienteExistente != null)
                {
                    _logger.LogWarning("CPF já cadastrado: {cpf}", dto.Paciente.Cpf);
                    throw new ArgumentException($"CPF {dto.Paciente.Cpf} já está em uso.");
                }
            }

            var novoUsuario = new Usuario
            {
                NomeCompleto = dto.NomeCompleto,
                NomeExibicao = dto.NomeExibicao,
                DataNascimento = dto.DataNascimento,
                Email = dto.Email,
                UserName = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                IsMedico = dto.IsMedico,
                IsPaciente = dto.IsPaciente
            };

            var result = await _userManager.CreateAsync(novoUsuario, dto.Password);
            if (!result.Succeeded)
            {
                _logger.LogError("Falha ao criar usuário. Erros: {erros}", string.Join(", ", result.Errors.Select(e => e.Description)));
                throw new Exception("Erro ao criar usuário");
            }

            _logger.LogInformation("Usuário criado com sucesso. ID: {id}, Email: {email}", novoUsuario.Id, novoUsuario.Email);

            string role = dto.IsMedico ? "MEDICO" :
              dto.IsPaciente ? "PACIENTE" : "RECEPCAO";

            await _userManager.AddToRoleAsync(novoUsuario, role);

            // Validação de CRM único para médico e criação do médico
            if (dto.IsMedico && dto.Medico != null)
            {
                _logger.LogInformation("Criando médico para o usuário ID: {id}", novoUsuario.Id);

                if (string.IsNullOrWhiteSpace(dto.Medico.CRM))
                {
                    _logger.LogWarning("CRM do médico não informado.");
                    throw new ArgumentException("CRM do médico é obrigatório.");
                }

                // Verificar se já existe um médico com o mesmo CRM
                var medicoExistente = await _medicoService.ObterMedicoPorCRM(dto.Medico.CRM);
                if (medicoExistente != null)
                {
                    _logger.LogWarning("CRM já cadastrado: {crm}", dto.Medico.CRM);
                    throw new ArgumentException($"CRM {dto.Medico.CRM} já está em uso.");
                }

                var especialidade = await _especialidadeService.ObterEspecialidadePorId(dto.Medico.EspecialidadeId);
                if (especialidade == null)
                {
                    _logger.LogWarning("Especialidade inválida para ID: {id}", dto.Medico.EspecialidadeId);
                    throw new ArgumentException("Especialidade inválida.");
                }

                var medicoCriado = await _medicoService.CriarMedico(novoUsuario.Id, dto.Medico);
                if (medicoCriado == null)
                {
                    _logger.LogError("Erro ao criar médico para o usuário ID: {id}", novoUsuario.Id);
                    throw new Exception("Erro ao criar médico.");
                }

                _logger.LogInformation("Médico criado com sucesso para o usuário ID: {id}", novoUsuario.Id);
            }

            if (dto.IsPaciente && dto.Paciente != null)
            {
                _logger.LogInformation("Criando paciente para o usuário ID: {id}", novoUsuario.Id);

                var pacienteCriado = await _pacienteService.CriarPaciente(novoUsuario.Id, dto.Paciente);
                if (pacienteCriado == null)
                {
                    _logger.LogError("Erro ao criar paciente para o usuário ID: {id}", novoUsuario.Id);
                    throw new Exception("Erro ao criar paciente.");
                }

                _logger.LogInformation("Paciente criado com sucesso para o usuário ID: {id}", novoUsuario.Id);
            }

            return new VisualizarUsuarioDto
            {
                Id = novoUsuario.Id,
                NomeCompleto = novoUsuario.NomeCompleto,
                NomeExibicao = novoUsuario.NomeExibicao,
                DataNascimento = novoUsuario.DataNascimento,
                Email = novoUsuario.Email ?? "",
                PhoneNumber = novoUsuario.PhoneNumber,
                Medico = dto.IsMedico && dto.Medico != null ? new VisualizarMedicoDto
                {
                    CRM = dto.Medico.CRM,
                    Especialidade = (await _especialidadeService.ObterEspecialidadePorId(dto.Medico.EspecialidadeId))?.Nome ?? ""
                } : null,
                Paciente = dto.IsPaciente && dto.Paciente != null ? new VisualizarPacienteDto
                {
                    Cpf = dto.Paciente.Cpf
                } : null,
                IsMedico = dto.IsMedico,
                IsPaciente = dto.IsPaciente
            };
        }

        // ======================
        // EDIT
        // ======================
        public async Task<VisualizarUsuarioDto?> EditarUsuario(Guid id, EditarUsuarioDto dto)
        {
            _logger.LogInformation("Iniciando edição do usuário ID: {id}", id);

            var usuarioAtual = await _userManager.FindByIdAsync(id.ToString());
            if (usuarioAtual == null)
            {
                _logger.LogWarning("Nenhum usuário encontrado para atualização. ID: {id}", id);
                throw new ArgumentException($"Nenhum usuário encontrado com id: {id}");
            }

            // Validação de email único (se estiver alterando o email)
            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != usuarioAtual.Email)
            {
                var emailExistente = await _userManager.FindByEmailAsync(dto.Email);
                if (emailExistente != null)
                {
                    _logger.LogWarning("Erro ao editar: email já está em uso: {email}", dto.Email);
                    throw new ArgumentException($"Usuário com o email {dto.Email} já existe.");
                }
            }

            // Validação de CPF único para paciente
            if (dto.IsPaciente && dto.Paciente != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Paciente.Cpf))
                {
                    _logger.LogWarning("CPF do paciente não informado.");
                    throw new ArgumentException("CPF é obrigatório.");
                }

                if (!ValidarCPF(dto.Paciente.Cpf))
                {
                    _logger.LogWarning("CPF inválido informado: {cpf}", dto.Paciente.Cpf);
                    throw new ArgumentException("CPF inválido.");
                }

                // Verificar se já existe um paciente com o mesmo CPF (que não seja o atual)
                var pacienteExistente = await _pacienteService.ObterPacientePorCpf(dto.Paciente.Cpf);
                if (pacienteExistente != null)
                {
                    // Obter o paciente atual do usuário (se existir) para comparar
                    var pacienteAtual = await ObterPacienteAtualDoUsuario(id);
                    if (pacienteAtual == null || pacienteExistente.Cpf != pacienteAtual.Cpf)
                    {
                        _logger.LogWarning("CPF já cadastrado: {cpf}", dto.Paciente.Cpf);
                        throw new ArgumentException($"CPF {dto.Paciente.Cpf} já está em uso.");
                    }
                }
            }

            _logger.LogDebug("Valores atuais: NomeCompleto={nome}, NomeExibicao={exibicao}, Email={email}",
                usuarioAtual.NomeCompleto, usuarioAtual.NomeExibicao, usuarioAtual.Email);

            usuarioAtual.NomeCompleto = dto.NomeCompleto ?? usuarioAtual.NomeCompleto;
            usuarioAtual.NomeExibicao = dto.NomeExibicao ?? usuarioAtual.NomeExibicao;
            usuarioAtual.DataNascimento = dto.DataNascimento ?? usuarioAtual.DataNascimento;

            // Atualizar email apenas se fornecido e diferente
            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != usuarioAtual.Email)
            {
                usuarioAtual.Email = dto.Email;
                usuarioAtual.UserName = dto.Email;
            }

            usuarioAtual.PhoneNumber = dto.PhoneNumber ?? usuarioAtual.PhoneNumber;

            var result = await _userManager.UpdateAsync(usuarioAtual);
            if (!result.Succeeded)
            {
                _logger.LogError("Falha ao atualizar usuário ID: {id}. Erros: {erros}", id, string.Join(", ", result.Errors.Select(e => e.Description)));
                throw new Exception("Erro ao atualizar usuário.");
            }

            // Validação de CRM único para médico e edição do médico
            if (dto.IsMedico && dto.Medico != null)
            {
                _logger.LogInformation("Editando dados de médico para usuário ID: {id}", id);

                if (string.IsNullOrWhiteSpace(dto.Medico.CRM))
                {
                    _logger.LogWarning("CRM do médico não informado.");
                    throw new ArgumentException("CRM do médico é obrigatório.");
                }

                // Verificar se já existe um médico com o mesmo CRM (que não seja o atual)
                var medicoExistente = await _medicoService.ObterMedicoPorCRM(dto.Medico.CRM);
                if (medicoExistente != null)
                {
                    // Obter o médico atual do usuário (se existir) para comparar
                    var medicoAtual = await ObterMedicoAtualDoUsuario(id);
                    if (medicoAtual == null || medicoExistente.CRM != medicoAtual.CRM)
                    {
                        _logger.LogWarning("CRM já cadastrado: {crm}", dto.Medico.CRM);
                        throw new ArgumentException($"CRM {dto.Medico.CRM} já está em uso.");
                    }
                }

                var especialidade = await _especialidadeService.ObterEspecialidadePorId(dto.Medico.EspecialidadeId);
                if (especialidade == null)
                {
                    _logger.LogWarning("Especialidade inválida para ID: {id}", dto.Medico.EspecialidadeId);
                    throw new ArgumentException("Especialidade inválida.");
                }

                await _medicoService.EditarMedico(id, dto.Medico);
            }

            if (dto.IsPaciente && dto.Paciente != null)
            {
                _logger.LogInformation("Editando dados de paciente para usuário ID: {id}", id);
                await _pacienteService.EditarPaciente(id, dto.Paciente);
            }

            _logger.LogInformation("Usuário atualizado com sucesso. ID: {id}", id);

            return new VisualizarUsuarioDto
            {
                Id = usuarioAtual.Id,
                NomeCompleto = usuarioAtual.NomeCompleto,
                NomeExibicao = usuarioAtual.NomeExibicao,
                DataNascimento = usuarioAtual.DataNascimento,
                Email = usuarioAtual.Email ?? "",
                PhoneNumber = usuarioAtual.PhoneNumber,
                Medico = dto.IsMedico && dto.Medico != null ? new VisualizarMedicoDto
                {
                    CRM = dto.Medico.CRM,
                    Especialidade = (await _especialidadeService.ObterEspecialidadePorId(dto.Medico.EspecialidadeId))?.Nome ?? ""
                } : null,
                Paciente = dto.IsPaciente && dto.Paciente != null ? new VisualizarPacienteDto
                {
                    Cpf = dto.Paciente.Cpf
                } : null,
                IsMedico = dto.IsMedico,
                IsPaciente = dto.IsPaciente
            };
        }

        // Método auxiliar para obter o paciente atual do usuário
        private async Task<VisualizarPacienteDto?> ObterPacienteAtualDoUsuario(Guid usuarioId)
        {
            var usuarioCompleto = await _userManager.Users
                .Include(u => u.Paciente)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            return usuarioCompleto?.Paciente != null
                ? new VisualizarPacienteDto { Cpf = usuarioCompleto.Paciente.Cpf }
                : null;
        }

        // Método auxiliar para obter o médico atual do usuário
        private async Task<VisualizarMedicoDto?> ObterMedicoAtualDoUsuario(Guid usuarioId)
        {
            var usuarioCompleto = await _userManager.Users
                .Include(u => u.Medico)
                .ThenInclude(m => m.Especialidade)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            return usuarioCompleto?.Medico != null
                ? new VisualizarMedicoDto
                {
                    CRM = usuarioCompleto.Medico.CRM,
                    Especialidade = usuarioCompleto.Medico.Especialidade?.Nome ?? ""
                }
                : null;
        }

        // ======================
        // ALTERAR SENHA
        // ======================
        public async Task<bool> AlterarSenha(string email, AlterarSenhaDto dto)
        {
            _logger.LogInformation("Iniciando alteração de senha para email: {email}", email);

            var usuario = await _userManager.FindByEmailAsync(email);
            if (usuario == null)
            {
                _logger.LogWarning("Usuário não encontrado para email: {email}", email);
                return false;
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(usuario);
            var result = await _userManager.ResetPasswordAsync(usuario, resetToken, dto.NovaSenha);

            if (!result.Succeeded)
            {
                _logger.LogError("Falha ao alterar senha para usuário ID: {id}. Erros: {erros}", usuario.Id,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
                return false;
            }

            _logger.LogInformation("Senha alterada com sucesso para usuário ID: {id}", usuario.Id);
            return true;
        }

        // ======================
        // DELETE (soft)
        // ======================
        public async Task<bool> ExcluirUsuario(Guid id)
        {
            _logger.LogInformation("Iniciando exclusão do usuário ID: {id}", id);

            var usuario = await _userManager.FindByIdAsync(id.ToString());
            if (usuario == null)
            {
                _logger.LogWarning("Usuário não encontrado para exclusão. ID: {id}", id);
                return false;
            }

            usuario.Status = false;
            var result = await _userManager.UpdateAsync(usuario);

            if (!result.Succeeded)
            {
                _logger.LogError("Falha ao excluir usuário ID: {id}. Erros: {erros}", id,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
                return false;
            }

            var tipoUsuario = await ObterUsuarioPorId(id);

            if (tipoUsuario?.IsMedico == true)
            {
                _logger.LogInformation("Excluindo dados de médico para usuário ID: {id}", id);
                await _medicoService.ExcluirMedico(id);
            }

            if (tipoUsuario?.IsPaciente == true)
            {
                _logger.LogInformation("Excluindo dados de paciente para usuário ID: {id}", id);
                await _pacienteService.ExcluirPaciente(id);
            }

            _logger.LogInformation("Usuário excluído com sucesso (soft delete). ID: {id}", id);
            return true;
        }

        private bool ValidarCPF(string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());
            return cpf.Length == 11;
        }
    }
}
