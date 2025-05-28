#!/bin/bash
if [ ! -f ".env" ]; then
  echo "📄 Criando .env a partir de env.example..."
  cp env.example .env
fi

echo "🚀 Subindo containers..."
docker-compose up --build
