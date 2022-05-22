using Microsoft.Extensions.Configuration;
using NLog;
using Oanda.RestApi.Services;

namespace BasicOandaApp.ConsoleApp;

internal static class InitializationSequence
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private static readonly IConfiguration configuration;

    private static readonly OandaRestApi restApi;

    private static readonly string dataDirectoryPath;

    static InitializationSequence()
    {
        AppState.ReadConfigurationValues();

        AppState.SetupApiClients();

        AppState.SetupDataDirectory();

        configuration = AppState.Configuration;

        restApi = AppState.OandaRestApi;

        dataDirectoryPath = AppState.DataDirectoryPath;
    }

    internal static async Task RunAsync()
    {
        CreateDataPathIfNotExists(dataDirectoryPath);

        await GetTradingAccountDetailAsync();

        await GetTradableInstrumentsAsync();

        log.Info("Initialization sequence complete");
    }

    private static void CreateDataPathIfNotExists(string dataPath)
    {
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);

            log.Info("{dataPath} created.", dataPath);
        }

        log.Info("{dataPath} exists.", dataPath);
    }

    private static async Task GetTradingAccountDetailAsync()
    {
        AppState.TradingAccount = 
            await restApi.GetAccountDetailsAsync(AppState.OandaAccountId);
    }

    private static async Task GetTradableInstrumentsAsync()
    {
        AppState.TradableInstruments = 
            await restApi.GetTradableInstrumentListAsync(AppState.OandaAccountId);
    }
}
