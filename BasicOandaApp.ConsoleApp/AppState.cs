using Microsoft.Extensions.Configuration;
using NLog;
using Oanda.RestApi.Models;
using Oanda.RestApi.Services;
using System.Reflection;

namespace BasicOandaApp.ConsoleApp;

internal static class AppState
{
    const string APP_SETTINGS_CONFIGURATION_FILE = "appsettings.json";

    const string OANDA_PRACTICE_API_KEY_CONFIGURATION_KEY = "oanda:practice:ApiKey";
    const string OANDA_ACCOUNT_ID_CONFIGURATION_KEY = "oanda:account:id";
    const string OANDA_REST_API_URL_CONFIGURATION_KEY = "oanda:restApiUrl";
    const string OANDA_STREAMING_API_URL_CONFIGURATION_KEY = "oanda:streamingApiUrl";

    const string APPLICATION_DATA_PATH_CONFIGURATION_KEY = "application:dataPath";

    public static IConfiguration Configuration 
    { 
        get => configuration ?? throw new NullReferenceException();
    }

    public static Logger Log
    {
        get => log ?? throw new NullReferenceException();
        set => log = value;
    }

    public static OandaRestApi OandaRestApi
    {
        get => oandaRestApi ?? throw new NullReferenceException();
        set => oandaRestApi = value;
    }

    public static string OandaRestApiUrl
    {
        get => oandaRestApiUrl ?? throw new NullReferenceException();
        set => oandaRestApiUrl = value;
    }

    public static string OandaStreamingApiUrl
    {
        get => oandaStreamingApiUrl ?? throw new NullReferenceException();
        set => oandaStreamingApiUrl = value;
    }

    public static string OandaAccountId
    {
        get => oandaAccountId ?? throw new NullReferenceException();
        set => oandaAccountId = value;
    }

    public static string OandaPracticeApiKey
    {
        get => oandaPracticeApiKey ?? throw new NullReferenceException();
        set => oandaPracticeApiKey = value;
    }
    public static string DataDirectoryPath
    {
        get => dataDirectoryPath ?? throw new NullReferenceException();
        set => dataDirectoryPath = value;
    }

    public static Account TradingAccount
    {
        get => tradingAccount ?? throw new NullReferenceException();
        set => tradingAccount = value;
    }

    public static IEnumerable<Instrument> TradableInstruments
    {
        get => tradableInstruments ?? throw new NullReferenceException();
        set => tradableInstruments = value;
    }

    internal static void SetupDataDirectory()
    {
        dataDirectoryPath = ResolvedPath(APPLICATION_DATA_PATH_CONFIGURATION_KEY);
    }

    private static string ResolvedPath(string configurationKey)
    {
        string dataPathConfigurationValue = AppState.GetConfigurationValue(configurationKey);

        string resolvedPath = Path.GetFullPath(dataPathConfigurationValue);

        log?.Info("{dataPathConfigurationValue} = {resolvedPath}", dataPathConfigurationValue, resolvedPath);

        return resolvedPath;
    }

    internal static void SetupApiClients()
    {
        AppState.OandaRestApi = new OandaRestApi(
            AppState.OandaRestApiUrl,
            AppState.OandaStreamingApiUrl,
            AppState.OandaPracticeApiKey);
    }

    internal static void ReadConfigurationValues()
    {
        AppState.OandaRestApiUrl = GetConfigurationValue(OANDA_REST_API_URL_CONFIGURATION_KEY);

        AppState.OandaStreamingApiUrl = GetConfigurationValue(OANDA_STREAMING_API_URL_CONFIGURATION_KEY);

        AppState.OandaAccountId = GetConfigurationValue(OANDA_ACCOUNT_ID_CONFIGURATION_KEY);

        AppState.OandaPracticeApiKey = GetConfigurationValue(OANDA_PRACTICE_API_KEY_CONFIGURATION_KEY);
    }

    public static string GetConfigurationValue(string configurationKey)
    {
        string configurationValue = Configuration[configurationKey].Trim();

        log?.Info("{configurationKey} = {configurationValue}", configurationKey, configurationValue);

        return string.IsNullOrWhiteSpace(configurationValue) ? throw new InvalidOperationException() : configurationValue;
    }

    private static readonly IConfiguration? configuration = new ConfigurationBuilder()
        .AddJsonFile(APP_SETTINGS_CONFIGURATION_FILE, optional: false, reloadOnChange: true)
        .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
        .Build();

    private static Logger? log = null;
    private static OandaRestApi? oandaRestApi = null;
    private static string? oandaRestApiUrl = null;
    private static string? oandaStreamingApiUrl = null;
    private static string? oandaAccountId = null;
    private static string? oandaPracticeApiKey = null;
    private static string? dataDirectoryPath = null;
    private static Account? tradingAccount = null;
    private static IEnumerable<Instrument>? tradableInstruments = null;
}
