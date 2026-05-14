// Arquivo principal de deployment
targetScope = 'subscription'

param location string = 'brazilsouth'
param projectName string = 'codex'
param environment string = 'prod'

// Parâmetros para secrets
param neonConnectionString string

// Variáveis
var resourceGroupName = '${projectName}-rg'
var containerAppEnvName = '${projectName}-cae'
var containerAppName = '${projectName}-api'
var appServicePlanName = '${projectName}-asp'
var appServiceName = '${projectName}-web'
var containerRegistryName = '${projectName}acr'
var keyVaultName = '${projectName}-kv-${uniqueString(subscription().id)}'
var userAssignedIdentityName = '${projectName}-umi'
var appInsightsName = '${projectName}-insights'
var logAnalyticsName = '${projectName}-logs'

// Criar Resource Group
resource resourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: resourceGroupName
  location: location
}

// Módulo para criar todos os recursos
module infrastructure 'modules/infrastructure.bicep' = {
  scope: resourceGroup
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

output resourceGroupId string = resourceGroup.id
output containerRegistryUrl string = infrastructure.outputs.containerRegistryUrl
output containerAppUrl string = infrastructure.outputs.containerAppUrl
output appServiceUrl string = infrastructure.outputs.appServiceUrl
output keyVaultId string = infrastructure.outputs.keyVaultId
