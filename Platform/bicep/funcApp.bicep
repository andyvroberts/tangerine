param appLocation string
param appName string
param hostingPlanId string
param storageAccId string
param storageAccApiVersion string
param storageAccName string
param resourceTags object
param workerRuntime string
param instrumentationKey string

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
      ]
      ftpsState: 'FtpsOnly'
      minTlsVersion: '1.2'
    }
    httpsOnly: true
  }
  tags: resourceTags
}
