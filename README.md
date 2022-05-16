# BasicOandaApp

A basic .NET Core Oanda console application use for simple deployments in Kubernetes.

```ps1: In C:\src\github.com\ongzhixian\BasicOandaApp
dotnet new sln -n BasicOandaApp
dotnet new console -n BasicOandaApp.ConsoleApp
dotnet sln .\BasicOandaApp.sln add .\BasicOandaApp.ConsoleApp\


dotnet add .\BasicOandaApp.ConsoleApp\ package Microsoft.Extensions.Configuration
dotnet add .\BasicOandaApp.ConsoleApp\ package Microsoft.Extensions.Configuration.UserSecrets
dotnet add .\BasicOandaApp.ConsoleApp\ package Microsoft.Extensions.Configuration.Json

Microsoft.Extensions.Http 

dotnet user-secrets --project .\BasicOandaApp.ConsoleApp\ init
dotnet user-secrets --project .\BasicOandaApp.ConsoleApp\ set "oanda:practice:ApiKey" "<api-key>"
dotnet user-secrets --project .\BasicOandaApp.ConsoleApp\ set "oanda:account:id" "<account-id>"


```


Other ways to extend configuration

```

Microsoft.Extensions.Configuration.CommandLine 
Microsoft.Extensions.Configuration.Binder 
Microsoft.Extensions.Configuration.EnvironmentVariables
```

## 

"Oanda": {
    "test": {
        "restApiUrl": " https://api-fxpractice.oanda.com",
        "streamingApiUrl": " https://stream-fxpractice.oanda.com/"
    },
    "productionUrl": {
        "restApiUrl": " https://api-fxtrade.oanda.com",
        "streamingApiUrl": "https://stream-fxtrade.oanda.com/"
    }
}