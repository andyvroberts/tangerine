// In order to build and deploy this funciton app, the main Tangerine resource group must already exist.
// If creating an RG, the deploymeht must have a subscription scope, whereas this deployment is resource group scoped.

// param to be passed in on execution. Override this param if using for Production.
@description('Non-prod is for all Dev and Test environments. Reduce costs by making this distinction.')
@allowed([
  'nonprod'
  'prod'
])
param environmentType string = 'nonprod'

// param may be overriden by providing values from outside the template file.
@description('Deploy at default resource group location (set in the CLI defaults).')
param location string = resourceGroup().location

param storagePrefix string = 'tangdata'

@description('Storage Account type')
param storageTypeSku string = (environmentType == 'prod') ? 'Standard_ZRS' : 'Standard_LRS'

@description('Storage account name is globally unique, but consistent across multiple deployments.')
var storageAccountName = '${storagePrefix}${uniqueString(resourceGroup().id)}'

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-05-01' = {
  name: storageAccountName
  location: location
  kind: 'StorageV2'
  sku: {
    name: storageTypeSku
  }
  properties: {
    accessTier: 'Hot'
    supportsHttpsTrafficOnly: true
  }
}

@description('The name of the function app that you wish to create.')
param appName string = 'TangerineFunc001'

//var functionAppName = appName
var hostingPlanName = appName

@description('Create the App Service Plan for the Server Farm.')
resource hostingPlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: hostingPlanName
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
  properties: {}
}

output storageAccountId string = storageAccount.name
