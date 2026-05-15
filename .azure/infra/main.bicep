// Arquivo principal de deployment
targetScope = 'resourceGroup'

param location string = 'brazilsouth'
param projectName string = 'codex'
param environment string = 'prod'

// Parâmetros para secrets
param neonConnectionString string

// Variáveis
var containerAppEnvName = '${projectName}-cae'
var containerAppName = '${projectName}-api'
var appServicePlanName = '${projectName}-asp'
var appServiceName = '${projectName}-web'
var containerRegistryName = '${projectName}acr2'
var keyVaultName = '${projectName}-kv-${uniqueString(resourceGroup().id)}'
var userAssignedIdentityName = '${projectName}-umi'
var appInsightsName = '${projectName}-insights'
var logAnalyticsName = '${projectName}-logs'

// Módulo para criar todos os recursos (no resource group existente)
module infrastructure 'modules/infrastructure.bicep' = {
  name: 'infrastructure'
  params: {
    location: location
    projectName: projectName
    environment: environment
    neonConnectionString: neonConnectionString
    containerAppEnvName: containerAppEnvName
    containerAppName: containerAppName
    appServicePlanName: appServicePlanName
    appServiceName: appServiceName
    containerRegistryName: containerRegistryName
    keyVaultName: keyVaultName
    userAssignedIdentityName: userAssignedIdentityName
    appInsightsName: appInsightsName
    logAnalyticsName: logAnalyticsName
  }
}

output resourceGroupName string = resourceGroup().name
output containerRegistryUrl string = infrastructure.outputs.containerRegistryUrl
output containerAppUrl string = infrastructure.outputs.containerAppUrl
output appServiceUrl string = infrastructure.outputs.appServiceUrl
output keyVaultId string = infrastructure.outputs.keyVaultId
