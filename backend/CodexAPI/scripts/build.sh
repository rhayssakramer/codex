#!/bin/bash
set -e

echo "Building .NET application..."
cd CodexAPI
dotnet restore
dotnet build -c Release
dotnet publish -c Release -o ../publish

echo "Running migrations..."
dotnet ef database update --context CodexDbContext || true

echo "Build completed successfully!"
