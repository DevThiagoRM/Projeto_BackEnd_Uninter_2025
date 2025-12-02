📘 API – Sistema Hospitalar

API desenvolvida para o projeto acadêmico do curso Tecnologia em Análise e Desenvolvimento de Sistemas do Centro Universitário Internacional Uninter.
O sistema foi construído para atender às necessidades da instituição fictícia VidaPlus, responsável pela gestão de hospitais, clínicas e serviços de home care.

O objetivo principal é fornecer uma API RESTful moderna, escalável e segura para gerenciar:

Usuários

Médicos

Pacientes

Consultas médicas

🧑‍🎓 Informações Acadêmicas

Autor: Thiago Recetto Moraes
Disciplina: Desenvolvimento Back-End
Curso: Tecnólogo em Análise e Desenvolvimento de Sistemas
Instituição: Centro Universitário Internacional Uninter
Orientador: Prof. Me. Winston Sen Lun Fung
Ano: 2025

📄 📚 Resumo do Projeto

Este projeto implementa uma API RESTful completa para um Sistema de Gestão Hospitalar e Serviços de Saúde (SGHSS), permitindo o gerenciamento eficiente e seguro de informações médicas — incluindo autenticação, cadastro de profissionais, pacientes e controle de consultas.

A solução foi desenvolvida utilizando:

Arquitetura Clean Architecture

Princípios de DDD (Domain-Driven Design)

Autenticação JWT

Persistência com Entity Framework Core

Banco de dados SQL Server

Documentação automática via Swagger

Registro de logs com ILogger

O sistema foi projetado para refletir desafios reais de ambientes hospitalares, como segurança de dados, agendamento sem conflitos e conformidade com a LGPD.

🛠️ Tecnologias Utilizadas
Tecnologia	Descrição
.NET 8 (Web API)	Framework principal da aplicação
Entity Framework Core	ORM utilizado
SQL Server	Banco de dados
Identity + JWT	Autenticação e controle de acesso
Swagger (Swashbuckle)	Documentação interativa
Clean Architecture + DDD	Organização técnica do projeto
ILogger	Registro de logs da aplicação
🔐 Autenticação e Segurança

Login via JWT

Renovação de token (Refresh Token)

Criptografia de senha com Identity e hashing

Papéis (roles):

ADMIN

MEDICO

PACIENTE

RECEPCAO

Restrições de acesso por função

Conformidade com princípios da LGPD

📌 Funcionalidades da API
✔️ Gestão de Usuários

Criar, editar e desativar usuários (soft delete)

Filtragem e listagem

Trocar senha

Obter por ID ou e-mail

✔️ Gestão de Médicos

Cadastro e edição

CRM e especialidades

Associação com usuário

✔️ Gestão de Pacientes

Cadastro e edição

CPF único

Associação com usuário

✔️ Consultas

Agendamento com validação de conflitos (médico e paciente)

Cancelamento com motivo

Listagem por:

Médico

Paciente

Intervalo de datas

📑 Regras de Negócio (Resumo)

E-mail, CPF e CRM devem ser únicos

Apenas ADMIN/RECEPCAO podem criar consultas

Médico e Paciente não podem ter duas consultas no mesmo horário

Consultas possuem duração mínima de 20 minutos

Usuários inativos não acessam o sistema

Exclusão é sempre lógica (soft delete)

🧱 Arquitetura do Projeto

A solução segue uma arquitetura limpa, com separação clara de responsabilidades:

/Application
    /Dtos
    /Services
    /Interfaces
/Domain
    /Entities
    /Enums
    /Repositories
/Infrastructure
    /Data
    /Repositories
    /Configurations
/Api
    Program.cs
    Controllers


Benefícios:

Alta testabilidade

Baixo acoplamento

Alta manutenibilidade

Escalável para módulos futuros (prontuário, exames, internações etc.)

▶️ Como Executar o Projeto
1. Clonar o repositório
git clone https://github.com/SEU-USUARIO/NOME-REPOSITORIO.git

2. Configurar o banco de dados (SQL Server)

Atualize o appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=VidaPlusDB;Trusted_Connection=True;TrustServerCertificate=True"
}

3. Aplicar migrações
dotnet ef database update

4. Rodar a API
dotnet run

5. Acessar Swagger

Abra no navegador:

https://localhost:7202/swagger

🧪 Testando a API

A API contém endpoints como:

🔸 Autenticação

POST /api/Auth/login

POST /api/Auth/refresh-token

POST /api/Auth/logout

🔸 Usuários

GET /api/Usuarios

GET /api/Usuarios/{id}

POST /api/Usuarios

PUT /api/Usuarios/{id}

DELETE /api/Usuarios/{id}

🔸 Consultas

POST /api/Consultas

GET /api/Consultas/medico/{id}

GET /api/Consultas/paciente/{id}

🧾 Requisitos Funcionais e Não Funcionais

O README também pode linkar para um arquivo docs/requisitos.md.

✔ RF001…RF021
✔ RNF001…RNF008
✔ Regras de Negócio validadas em todos os endpoints

📊 Casos de Uso

Este projeto implementa casos de uso como:

Cadastro de Usuário

Autenticação JWT

Cadastro de Médico

Cadastro de Paciente

Agendamento de Consulta

Cancelamento de Consulta

Pesquisa por filtros

(Recomenda-se manter um arquivo docs/casos_de_uso.md para anexar ao repositório.)

🧾 Referências Bibliográficas
MICROSOFT. Documentação Oficial do .NET. Disponível em: https://learn.microsoft.com/dotnet/.  
MICROSOFT. Documentação do Entity Framework Core. Disponível em: https://learn.microsoft.com/ef/.  
MICROSOFT. Autenticação JWT no ASP.NET Core. Disponível em: https://learn.microsoft.com/aspnet/core/security/authentication/jwt.  
FOWLER, Martin. Patterns of Enterprise Application Architecture. Addison-Wesley, 2003.  
EVANS, Eric. Domain-Driven Design: Tackling Complexity in the Heart of Software. Addison-Wesley, 2004.  
SONMEZ, John. Clean Code in C#. Manning Publications, 2022.  
SERRANO, Milene. LGPD – Lei Geral de Proteção de Dados Pessoais. Ed. Campus, 2021.

❤️ Agradecimentos

Dedicado a todos que me apoiaram durante essa trajetória acadêmica e pessoal.

📬 Contato

Thiago Recetto Moraes
📧 thiago.social@outlook.com
