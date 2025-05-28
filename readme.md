# Desafio L2 .NET

## 🚀 Início Rápido

### Pré-requisitos
- Docker
- Docker Compose
- .NET SDK (opcional, apenas para desenvolvimento local)

### Passo a Passo

1. Clone o projeto
```bash
git clone https://github.com/danielfk11/Desafio-L2-.NET/
cd Desafio-L2-.NET
```

2. Configure o ambiente
```bash
cp env.example .env
```

3. Inicie o projeto
```bash
docker-compose up --build
```

A API estará disponível em: http://localhost:5050

## 🧪 Testes

```bash
# Usando Docker
docker-compose exec api dotnet test

# Ou localmente
cd Api.Tests
dotnet test
```

## 📦 Estrutura
```
.
├── Api/              # Projeto principal
├── Api.Tests/        # Testes
├── docker-compose.yml
└── env.example
```

## 🔧 Configuração

### Banco de Dados
- SQL Server 2022
- Porta: 1433
- Usuário: sa
- Senha: Definida no arquivo .env

### Variáveis de Ambiente
Copie o arquivo `env.example` para `.env` e ajuste conforme necessário:
```
CONNECTION_STRING=Server=db;Database=LojaManoel;User Id=sa;Password=Daniel123!;TrustServerCertificate=True;Encrypt=False
DB_SA_PASSWORD=Daniel123!
AMBIENTE=dev
```

## 📝 Licença
Este projeto está licenciado sob a licença incluída no arquivo LICENSE.
