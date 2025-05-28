# Base SDK para todas as etapas
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Final
FROM base AS final

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

# Copia o script de entrada e garante permissão
COPY entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

# Define pasta base de execução
WORKDIR /app/out

# Executa o script de entrada
ENTRYPOINT ["/app/entrypoint.sh"]
