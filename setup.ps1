# Script de configuração do projeto INSURANCES
# Execute como administrador se necessário

Write-Host "🚀 Configurando projeto INSURANCES..." -ForegroundColor Green

# Verificar se o Docker está rodando
Write-Host "🔍 Verificando Docker..." -ForegroundColor Yellow
try {
    docker version | Out-Null
    Write-Host "✅ Docker está rodando" -ForegroundColor Green
} catch {
    Write-Host "❌ Docker não está rodando. Inicie o Docker Desktop primeiro." -ForegroundColor Red
    exit 1
}

# Parar e remover containers existentes
Write-Host "🧹 Limpando containers existentes..." -ForegroundColor Yellow
docker stop insurances-sqlserver 2>$null
docker rm insurances-sqlserver 2>$null

# Iniciar SQL Server com Docker Compose
Write-Host "🐳 Iniciando SQL Server..." -ForegroundColor Yellow
docker-compose up -d

# Aguardar o SQL Server estar pronto
Write-Host "⏳ Aguardando SQL Server estar pronto..." -ForegroundColor Yellow
$maxAttempts = 30
$attempt = 0

do {
    $attempt++
    Write-Host "Tentativa $attempt/$maxAttempts..." -ForegroundColor Gray
    
    try {
        $result = docker exec insurances-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "@123456qwe" -Q "SELECT 1" 2>$null
        if ($LASTEXITCODE -eq 0) {
            Write-Host "✅ SQL Server está pronto!" -ForegroundColor Green
            break
        }
    } catch {
        # Ignorar erros durante a inicialização
    }
    
    Start-Sleep -Seconds 2
} while ($attempt -lt $maxAttempts)

if ($attempt -ge $maxAttempts) {
    Write-Host "❌ Timeout aguardando SQL Server" -ForegroundColor Red
    exit 1
}

# Restaurar pacotes NuGet
Write-Host "📦 Restaurando pacotes NuGet..." -ForegroundColor Yellow
dotnet restore

# Compilar solução
Write-Host "🔨 Compilando solução..." -ForegroundColor Yellow
dotnet build

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Erro na compilação" -ForegroundColor Red
    exit 1
}

# Executar migrations para a API de Propostas
Write-Host "🗄️ Executando migrations para API de Propostas..." -ForegroundColor Yellow
dotnet ef database update --project INSURANCES.DATA --startup-project INSURANCES.PROPOSAL.API

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Erro ao executar migrations para API de Propostas" -ForegroundColor Red
    exit 1
}

# Executar migrations para a API de Contratação
Write-Host "🗄️ Executando migrations para API de Contratação..." -ForegroundColor Yellow
dotnet ef database update --project INSURANCES.DATA --startup-project INSURANCES.HIRING.API

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Erro ao executar migrations para API de Contratação" -ForegroundColor Red
    exit 1
}

# Executar testes
Write-Host "🧪 Executando testes..." -ForegroundColor Yellow
dotnet test INSURANCE.TEST --verbosity minimal

Write-Host ""
Write-Host "🎉 Configuração concluída com sucesso!" -ForegroundColor Green
Write-Host ""
Write-Host "📋 Próximos passos:" -ForegroundColor Cyan
Write-Host "1. Execute a API de Propostas: cd INSURANCES.PROPOSAL.API && dotnet run" -ForegroundColor White
Write-Host "2. Execute a API de Contratação: cd INSURANCES.HIRING.API && dotnet run" -ForegroundColor White
Write-Host "3. Acesse o Swagger:" -ForegroundColor White
Write-Host "   - Propostas: https://localhost:5001/swagger" -ForegroundColor White
Write-Host "   - Contratação: https://localhost:5003/swagger" -ForegroundColor White
Write-Host ""
Write-Host "🐳 Para parar o SQL Server: docker-compose down" -ForegroundColor Yellow
Write-Host "🐳 Para ver logs: docker-compose logs -f sqlserver" -ForegroundColor Yellow
