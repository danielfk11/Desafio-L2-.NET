# Base SDK para todas as etapas
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Final (mantém SDK para rodar EF e iniciar app)
FROM base AS final
WORKDIR /app

# Instala netcat para esperar o SQL Server
RUN apt-get update && apt-get install -y netcat-openbsd

# Copia o código inteiro (inclusive .csproj) para dentro do container
COPY . .

# Instala EF CLI
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

# Restaura e publica para /app/out
RUN dotnet restore ./Api.csproj
RUN dotnet publish ./Api.csproj -c Release -o /app/out

# Copia o script de entrada
COPY entrypoint.sh ./entrypoint.sh
RUN chmod +x ./entrypoint.sh

# Define a pasta publicada como base de execução
WORKDIR /app/out

# Executa o script de entrada
ENTRYPOINT ["../entrypoint.sh"]
