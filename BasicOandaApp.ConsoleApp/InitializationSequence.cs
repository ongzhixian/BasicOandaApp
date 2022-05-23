using Microsoft.Extensions.Configuration;
using NLog;
using Oanda.RestApi.Services;

namespace BasicOandaApp.ConsoleApp;

internal static class InitializationSequence
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    // private static readonly IConfiguration configuration = AppState.Configuration;

    private static readonly OandaRestApi restApi = AppState.OandaRestApi;

    private static readonly string dataDirectoryPath = AppState.DataDirectoryPath;

    internal static async Task RunAsync()
    {
        CreateDataPathIfNotExists(dataDirectoryPath);

        await GetTradingAccountDetailAsync();

        await GetTradableInstrumentsAsync();

        log.Info("Initialization sequence complete");
    }

    private static void CreateDataPathIfNotExists(string dataPath)
    {
        if (Directory.Exists(dataPath))
        {
            log.Info("{dataPath} exists.", dataPath);

            return;
        }

        Directory.CreateDirectory(dataPath);

        log.Info("{dataPath} created.", dataPath);
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
