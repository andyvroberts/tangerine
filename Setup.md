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

Add csproj references
````
dotnet add package Microsoft.Azure.WebJobs.Extensions.Tables --prerelease
dotnet add package Microsoft.Azure.WebJobs.Extensions.Storage
```
  
T






