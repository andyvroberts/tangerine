targetScope = 'subscription'

// Resource Name Convention (use the MS cloud adoption standard)
// {Resource Type}-{Application}-{Environment}-<Region>-<Instance XXX>
// only use Region in region sensitive components such as for Networking, etc.
// Instance is also optional.

// the param file for acq_rg_location must contain only one of these 2 values.
@allowed([
  'uksouth'
  'ukwest'
])
param rgLocation string

@description('project name can be overriden by parameters.')
param projShortName string = 'tngrn'

@allowed([
  'nonprod'
  'dev'
  'test'
  'prod'
])
param envType string = 'nonprod'

@description('resource tags are specified in the parameter file params_<env>.json')
param resourceTags object

param instanceVersion string

var rg_name = 'rg-${projShortName}-${envType}-${instanceVersion}'

// Create the resource group to contain the Tangerine.
resource newRg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: rg_name
  location: rgLocation
  tags: resourceTags
}

param storageTypeSku string = (envType == 'prod') ? 'Standard_ZRS' : 'Standard_LRS'

var storage_name = 'st${projShortName}${envType}${instanceVersion}'

// Storage account
module storageAccount 'storageAcc.bicep' = {
  name: 'storageModule'
  scope: resourceGroup(newRg.name)
  params: {
    storageLocation: rgLocation
    storageName: storage_name
    storageSku: storageTypeSku
    resourceTags: resourceTags
  }
}

var app_plan_name = 'asp-${projShortName}-${envType}-${instanceVersion}'

// Hosting Storage Plan (server farm)
module hostingPlan 'hostServicePlan.bicep' = {
  name: 'hostingPlanModule'
  scope: resourceGroup(newRg.name)
  params: {
    hostingPlanName: app_plan_name
    hostingLocation: rgLocation
    resourceTags: resourceTags
  }
}

var insights_name = 'appi-${projShortName}-${envType}-${instanceVersion}'
param logRetentionPeriod int = 30

// Application Insights Instance
module appInsights 'insights.bicep' = {
  name: 'appInsightsModule'
  scope: resourceGroup(newRg.name)
  params: {
    insightsName: insights_name
    insightsLocation: rgLocation
    logRetention: logRetentionPeriod
    resourceTags: resourceTags
  }
}

// need the outputs from the previous modules.
var hostingPlanId = hostingPlan.outputs.planId
var storageAccId = storageAccount.outputs.storageId
var storageApiVersion = storageAccount.outputs.apiVersion
var appInsightsInstrKey = appInsights.outputs.instrumentationKey

// need existing resources for application settings (secrets).
@description('The business data storage account name is needed as a function app setting.')
param businessDataStorageName string
@description('the Bmrs API key is required as a function app setting.')
param businessDataRg string
param businessDataVaultName string
param bmrsApiKeySecretName string

@description('The name of the function app that you wish to create.')
param functionAppName string = 'func-${projShortName}-${envType}-${instanceVersion}'

@description('The language worker runtime to load in the function app.')
@allowed([
  'python'
  'dotnet'
  'java'
])
param runtime string = 'dotnet'

// Get the Bmrs API key from the Energy Data key vault.
// the key vault must have "enabledForTemplateDeployment: true" as a property.
resource businessVault 'Microsoft.KeyVault/vaults@2016-10-01' existing = {
  scope: resourceGroup(businessDataRg)
  name: businessDataVaultName
}

// Tangerine Function App plust app settings and secrets.
module functionApp 'funcApp.bicep' = {
  name: 'functionsModule'
  scope: resourceGroup(newRg.name)
  params: {
    appName: functionAppName
    appLocation: rgLocation
    storageAccId: storageAccId
    storageAccApiVersion: storageApiVersion
    storageAccName: storage_name
    businessDataStorageName: businessDataStorageName
    businessDataStorageRg: businessDataRg
    hostingPlanId: hostingPlanId
    workerRuntime: runtime
    instrumentationKey: appInsightsInstrKey
    bmrsApiKey: businessVault.getSecret(bmrsApiKeySecretName)
    resourceTags: resourceTags
  }
}
