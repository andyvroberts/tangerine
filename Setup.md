# Setup
Setup the function app scaffold.
```
func init Bmrs
cd Bmrs
func new --language C# --name GetSystemPrices --template "Timer trigger"
```

Set the cron expression to run daily at 2am.  
https://crontab.guru/examples.html  
```
[TimerTrigger("0 2 * * *")]
```

In order to run a C# timer trigger on startup, set the RunOnStartup attribute in the function declaration:  
```
public String GetSystemPriceWithVolume([TimerTrigger("0 2 * * *", RunOnStartup = true)] TimerInfo myTimer,
```
  
Add csproj references  
```
dotnet add package Microsoft.Azure.WebJobs.Extensions.Tables --prerelease
dotnet add package Microsoft.Azure.WebJobs.Extensions.Storage.Queues --version 5.0.1
dotnet add package Microsoft.Azure.WebJobs.Extensions.Storage
```

At this time, the table extensions has a dependancy on azure.data.table version 12.5.  

Somthing else to note, is that this version of webjobs.extensions.tables did not seem to contain the same amount of methods that you get within the azure.data.tables library.  In particular, it would have been nice to see TableClient.GetEntityIfExists() to handle ResourceNotFound errors.


## Function Bindings
Bindings allow you to interact with Azure storage resources without having to instantiate and manage SDK client lifetimes.

### Queue Storage
This is a good document page.  
https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-queue?tabs=in-process%2Cextensionv5%2Cextensionv3&pivots=programming-language-csharp

Notice how the host.json has a setting for queue message encoding, which you can set to Base64, and hence avoid having to apply conversions in the C# code of the Azure function you are writting.  See this reference:  
https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-queue-output?tabs=in-process%2Cextensionv5&pivots=programming-language-csharp#usage

MS are explicit in that they recommend using a **return** output binding only if a message must be sent on successful function completion:  
https://learn.microsoft.com/en-us/azure/azure-functions/functions-dotnet-class-library?tabs=v2%2Ccmd#binding-to-method-return-value

You can also look at the github function app queue samples.  
https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/storage/Microsoft.Azure.WebJobs.Extensions.Storage.Queues/samples/functionapp

### Table Storage
You can do one of these for input bindings:  
1. Bind to a storage table and use a TableClient
2. Bind to an entity within a storage table and use the referenced object (class representing the record) directly  

In this example, the first use of table storage is limited to reading a configuration record and then updating it at the end of the process.  So in this case, we are binding directly to an entity (record with the table).  

Note: If the entity of the binding does not exist in the table, the function will error.   The record should always be pre-configured in a new deploymet.     

Currently output bindings have the restictions:  
1. Can only be used for insert (not update)  

In this project, our process must be repeatable which means that our second usage of table storage is for 'Upsert' operations.  And for this we will directly use the Azure tables SDK instead of the function app bindings.    

## Bicep
Read the docs.  
https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/file

Make sure bicep is installed in the Azure CLI.
```
az bicep install && az bicep upgrade
```

Once you have created the bicep file, you will need to set the default resrouce group within the CLI. By doing this you will not need to specify it on every bicep declaration.  
```
az configure --defaults group=Tangerine001
```

Deploy the bicep file to Azure.  This will have the result of executing the template and creating whatever resources it specifies.  
```
az deployment group create --template-file tangerine.bicep
```

List the contents of the default resource group to see if your resources were created.  
```
az deployment group list --output table
az resource list --output table
```

## Azure Naming Conventions for Bicep Deployments
Use the cloud adoption framework standards.  
https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-naming

Also, MS has a set of standard abbreviations for Azure Resource Types:  
https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-abbreviations

In our case we are using these:
- rg = resource group
- asp = app service plan
- st = storage account
- appi = application insights
- func = function app  
   
### Bicep for Components
Storage Account:  
https://learn.microsoft.com/en-us/azure/templates/microsoft.storage/storageaccounts?pivots=deployment-language-bicep

Application Insights:  
https://learn.microsoft.com/en-us/azure/templates/microsoft.insights/components?pivots=deployment-language-bicep#applicationinsightscomponentproperties

Function Apps:
https://learn.microsoft.com/en-us/azure/templates/microsoft.web/sites?pivots=deployment-language-bicep

### Secure Variables
There are bicep linting rules that prevent you from extracting secrets and passwords, as these can be displayed within log files.  
https://learn.microsoft.com/en-gb/azure/azure-resource-manager/bicep/linter-rule-outputs-should-not-contain-secrets



