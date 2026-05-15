using './main.bicep'

// Arquivo de parâmetros para deployment
param location = 'brazilsouth'
param projectName = 'codex'
param environment = 'prod'

// IMPORTANTE: Substituir com sua string de conexão real do Neon
param neonConnectionString = 'postgresql://neondb_owner:npg_CgIEXAVT03kW@ep-square-paper-aqikzeh0-pooler.c-8.us-east-1.aws.neon.tech/neondb?sslmode=require&channel_binding=require'
