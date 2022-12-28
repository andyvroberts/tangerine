param storageLocation string
param storageName string
param storageSku string
param resourceTags object

resource storageAcc 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: storageName
  location: storageLocation
  sku: {
    name: storageSku
  }
  kind: 'StorageV2'
  properties: {
    accessTier: 'Hot'
    supportsHttpsTrafficOnly: true
  }
  tags: resourceTags
}

output storageId string = storageAcc.id
output apiVersion string = storageAcc.apiVersion
