# BasicOandaApp

A basic .NET Core Oanda console application use for simple deployments in Kubernetes.

```ps1: In C:\src\github.com\ongzhixian\BasicOandaApp
dotnet new sln -n BasicOandaApp
dotnet new console -n BasicOandaApp.ConsoleApp
dotnet sln .\BasicOandaApp.sln add .\BasicOandaApp.ConsoleApp\


dotnet add .\BasicOandaApp.ConsoleApp\ package Microsoft.Extensions.Configuration
dotnet add .\BasicOandaApp.ConsoleApp\ package Microsoft.Extensions.Configuration.UserSecrets
dotnet add .\BasicOandaApp.ConsoleApp\ package Microsoft.Extensions.Configuration.Json
dotnet add .\BasicOandaApp.ConsoleApp\ package Microsoft.Data.Analysis
dotnet add .\BasicOandaApp.ConsoleApp\ package MathNet.Numerics

dotnet add .\BasicOandaApp.ConsoleApp\ package NLog

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

## Secrets

Secrets that required by application

`"oanda:practice:ApiKey" "<api-key>"`

An Oanda REST API key; example value:

999af5dbe42ea92d0b5f4ae6450aa91a-999ae01ca759db0c5d6cf35651674bfd

`"oanda:account:id" "<account-id>"`

An Oanda account id; example value:

999-999-11176018-999




## Urls used by Oanda REST APIs

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

Reference: https://developer.oanda.com/rest-live-v20/development-guide/