param insightsName string
param insightsLocation string
param logRetention int
param resourceTags object

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: insightsName
  location: insightsLocation
  kind: 'web'
  properties: {
    Application_Type: 'web'
    Request_Source: 'rest'
    RetentionInDays: logRetention
  }
  tags: resourceTags
}

output instrumentationKey string = applicationInsights.properties.InstrumentationKey
