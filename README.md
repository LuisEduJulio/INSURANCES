# INSURANCES - Sistema de Seguros

Sistema de gerenciamento de seguros com APIs para propostas e contratos de contratação.

## 🏗️ Arquitetura

O projeto segue uma arquitetura em camadas com Clean Architecture:

- **INSURANCES.CORE**: Entidades, DTOs, interfaces e enums
- **INSURANCES.DATA**: Repositórios e contexto do Entity Framework
- **INSURANCES.APPLICATION**: Serviços de negócio
- **INSURANCES.IOC**: Injeção de dependências e configurações
- **INSURANCES.PROPOSAL.API**: API para gerenciamento de propostas
- **INSURANCES.HIRING.API**: API para gerenciamento de contratos
- **INSURANCE.TEST**: Testes unitários e de integração

## 🚀 Tecnologias

- **.NET 8.0**
- **Entity Framework Core 8.0**
- **SQL Server**
- **AutoMapper**
- **FluentValidation**
- **Swagger/OpenAPI**
- **xUnit** (testes)
- **Docker**

## 📋 Pré-requisitos

- .NET 8.0 SDK
- Docker Desktop
- SQL Server (via Docker)

## 🐳 Configuração com Docker

### 1. Iniciar SQL Server

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=@123456qwe" \
  -p 1433:1433 --name sqlserver --hostname sqlserver \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

### 2. Verificar se o container está rodando

```bash
docker ps
```

### 3. Conectar ao banco (opcional)

```bash
docker exec -it sqlserver /opt/mssql-tools/bin/sqlcmd \
  -S localhost -U sa -P "@123456qwe"
```

## 🗄️ Configuração do Banco de Dados

### 1. Executar as migrations

```bash
# Para a API de Propostas
dotnet ef database update --project INSURANCES.DATA --startup-project INSURANCES.PROPOSAL.API

# Para a API de Contratação
dotnet ef database update --project INSURANCES.DATA --startup-project INSURANCES.HIRING.API
```

### 2. Verificar conexão

As APIs estão configuradas para conectar ao SQL Server local na porta 1433.

## 🚀 Executando as APIs

### 1. API de Propostas

```bash
cd INSURANCES.PROPOSAL.API
dotnet run
```

**Porta**: 5000 (HTTP) / 5001 (HTTPS)
**Swagger**: https://localhost:5001/swagger

### 2. API de Contratação

```bash
cd INSURANCES.HIRING.API
dotnet run
```

**Porta**: 5002 (HTTP) / 5003 (HTTPS)
**Swagger**: https://localhost:5003/swagger

## 📚 Documentação da API

### 🔹 API de Propostas (INSURANCES.PROPOSAL.API)

#### Endpoints

| Método | Endpoint | Descrição | Parâmetros |
|--------|----------|------------|------------|
| `GET` | `/api/proposal/{id}` | Buscar proposta por ID | `id` (Guid) |
| `GET` | `/api/proposal/list` | Listar propostas com paginação | `page` (int), `count` (int) |
| `POST` | `/api/proposal` | Criar nova proposta | `PostProposalDto` |
| `PUT` | `/api/proposal/status` | Atualizar status da proposta | `PutProposalUpdateStatusByIdDto` |

#### DTOs

**PostProposalDto**
```json
{
  "name": "string",
  "proposal": 0.00
}
```

**PutProposalUpdateStatusByIdDto**
```json
{
  "id": "guid",
  "proposalStatus": "ANALYSIS|APPROVED|REJECTED"
}
```

**GetProposalListDto**
```json
{
  "page": 1,
  "count": 25
}
```

#### ModelViews

**ProposalModelView**
```json
{
  "id": "guid",
  "name": "string",
  "proposal": 0.00,
  "proposalStatus": "ANALYSIS|APPROVED|REJECTED",
  "isDisabled": false,
  "hiringId": "guid|null",
  "hiringDate": "datetime|null",
  "createdDate": "datetime",
  "updatedDate": "datetime|null"
}
```

**ProposalListModelView**
```json
{
  "proposals": [
    "ProposalModelView[]"
  ]
}
```

### 🔹 API de Contratação (INSURANCES.HIRING.API)

#### Endpoints

| Método | Endpoint | Descrição | Parâmetros |
|--------|----------|------------|------------|
| `GET` | `/api/hiring/{id}` | Buscar contrato por ID | `id` (Guid) |
| `POST` | `/api/hiring` | Criar novo contrato | `PostHiringDto` |

#### DTOs

**PostHiringDto**
```json
{
  "name": "string",
  "proposalId": "guid",
  "isApproved": false
}
```

#### ModelViews

**HiringModelView**
```json
{
  "id": "guid",
  "name": "string",
  "proposalId": "guid",
  "hiringDate": "datetime",
  "isApproved": false,
  "createdDate": "datetime",
  "updatedDate": "datetime|null"
}
```

## 🧪 Executando os Testes

```bash
# Executar todos os testes
dotnet test INSURANCE.TEST

# Executar com detalhes
dotnet test INSURANCE.TEST --verbosity normal

# Executar testes específicos
dotnet test INSURANCE.TEST --filter "FullyQualifiedName~HiringServiceTests"
```

### Tipos de Testes

- **Repository Tests**: Testam os repositórios com banco em memória
- **Service Tests**: Testam os serviços com mocks
- **Integration Tests**: Testam a integração entre camadas

## 🔧 Configurações

### Connection Strings

As APIs estão configuradas para usar as seguintes connection strings:

**Development**
```json
{
  "ConnectionStrings": {
    "SqlServerConnection": "Data Source=localhost;Initial Catalog=sqlserver;User Id=sa;Password=@123456qwe;MultipleActiveResultSets=true;TrustServerCertificate=true;Encrypt=false;"
  }
}
```

### Portas das APIs

- **Proposal API**: 5000/5001
- **Hiring API**: 5002/5003

## 📁 Estrutura do Projeto

```
INSURANCES/
├── INSURANCES.CORE/                 # Entidades, DTOs, interfaces
│   ├── Entities/                    # Entidades do domínio
│   ├── Dtos/                        # Data Transfer Objects
│   ├── ModelView/                   # Modelos de visualização
│   ├── Ports/                       # Interfaces (contratos)
│   └── Mappers/                     # Perfis do AutoMapper
├── INSURANCES.DATA/                 # Camada de dados
│   ├── Repositories/                # Implementações dos repositórios
│   ├── Factory/                     # DbContext e configurações
│   └── Migrations/                  # Migrations do EF Core
├── INSURANCES.APPLICATION/          # Serviços de aplicação
│   └── Services/                    # Implementações dos serviços
├── INSURANCES.IOC/                  # Injeção de dependências
│   ├── Dependecies/                 # Container de dependências
│   └── Register/                    # Registros de serviços
├── INSURANCES.PROPOSAL.API/         # API de propostas
│   └── Controllers/                 # Controllers da API
├── INSURANCES.HIRING.API/           # API de contratação
│   └── Controllers/                 # Controllers da API
└── INSURANCE.TEST/                  # Projeto de testes
    ├── Repositories/                # Testes dos repositórios
    ├── Services/                    # Testes dos serviços
    └── Integration/                 # Testes de integração
```

## 🚨 Regras de Negócio

### Propostas

1. **Criação**: Proposta é criada com status `ANALYSIS`
2. **Status**: Pode ser `ANALYSIS`, `APPROVED` ou `REJECTED`
3. **Validação**: Nome é obrigatório

### Contratos de Contratação

1. **Validação**: Só pode criar contrato para proposta `APPROVED`
2. **Unicidade**: Uma proposta só pode ter um contrato
3. **Dependência**: Contrato depende da existência da proposta

## 🔍 Swagger/OpenAPI

Ambas as APIs incluem documentação Swagger:

- **Proposal API**: https://localhost:5001/swagger
- **Hiring API**: https://localhost:5003/swagger

## 📊 Monitoramento

- **Logs**: Configurados com Microsoft.Extensions.Logging
- **Validação**: FluentValidation para validação de entrada
- **Mapeamento**: AutoMapper para conversão de objetos

## 🛠️ Comandos Úteis

```bash
# Restaurar pacotes
dotnet restore

# Compilar solução
dotnet build

# Executar testes
dotnet test

# Executar migrations
dotnet ef database update

# Criar nova migration
dotnet ef migrations add NomeMigration

# Limpar build
dotnet clean

# Publicar
dotnet publish -c Release
```

## 🐛 Troubleshooting

### Erro de Conexão SQL Server
- Verificar se o container Docker está rodando
- Verificar se a porta 1433 está livre
- Verificar se a senha está correta

### Erro de Porta em Uso
- Verificar se outra aplicação está usando a porta
- Alterar a porta no `launchSettings.json`

### Erro de Migration
- Verificar se o banco está acessível
- Verificar se as connection strings estão corretas

## 📝 Licença

Este projeto é para fins educacionais e de demonstração.

## 👥 Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature
3. Commit suas mudanças
4. Push para a branch
5. Abra um Pull Request
