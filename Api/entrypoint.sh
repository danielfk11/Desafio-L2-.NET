#!/bin/bash
set -e

echo "⏳ Aguardando o banco de dados iniciar na porta 1433..."

# Tenta conectar por até 30 vezes (60s)
for i in {1..30}; do
  if nc -z db 1433; then
    echo "✅ Conexão com o banco de dados estabelecida!"
    break
  fi
  echo "🔁 Tentativa $i: aguardando SQL Server..."
  sleep 2
done

# Falha após 30 tentativas
if ! nc -z db 1433; then
  echo "❌ Falha ao conectar ao SQL Server após múltiplas tentativas."
  exit 1
fi

echo "🏗 Aplicando migrations com Entity Framework..."
dotnet ef database update --project ../Api.csproj

echo "🚀 Iniciando aplicação..."
exec dotnet Api.dll
