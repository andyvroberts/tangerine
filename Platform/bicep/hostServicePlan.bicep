param hostingPlanName string
param hostingLocation string
param resourceTags object

@description('Create the App Service Plan for the Server Farm.')
resource hostingPlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: hostingPlanName
  location: hostingLocation
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
  properties: {}
  tags: resourceTags
}

output planId string = hostingPlan.id
