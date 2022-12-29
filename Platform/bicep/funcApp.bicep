param appLocation string
param appName string
param hostingPlanId string
param storageAccId string
param storageAccApiVersion string
param storageAccName string
param resourceTags object
param workerRuntime string
param instrumentationKey string
param businessDataStorageName string
param businessDataStorageRg string

// Get a reference to the existing business data storage account
resource businessData 'Microsoft.Storage/storageAccounts@2019-06-01' existing = {
  scope: resourceGroup(businessDataStorageRg)
  name: businessDataStorageName
}

// ensure the api key is not displayed in logging.
@secure()
param bmrsApiKey string

resource functionApp 'Microsoft.Web/sites@2021-03-01' = {
  name: appName
  location: appLocation
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: hostingPlanId
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          //value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storageAccId, storageAccApiVersion).keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          //value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storageAccId, storageAccApiVersion).keys[0].value}'
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: instrumentationKey
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: workerRuntime
        }
        {
          name: 'EnergyDataStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${businessData.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(businessData.id, businessData.apiVersion).keys[0].value}'
        }
        {
          name: 'BmrsApiKey'
          value: bmrsApiKey
        }
      ]
      ftpsState: 'FtpsOnly'
      minTlsVersion: '1.2'
    }
    httpsOnly: true
  }
  tags: resourceTags
}
