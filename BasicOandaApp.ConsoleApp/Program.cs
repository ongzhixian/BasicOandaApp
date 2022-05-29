using BasicOandaApp.ConsoleApp;
using BasicOandaApp.ConsoleApp.Models;
using Microsoft.Extensions.Configuration;
using NLog;
using Oanda.RestApi.Models;
using Oanda.RestApi.Services;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using BasicOandaApp.ConsoleApp.Extensions;
using Microsoft.Data.Analysis;
using BasicOandaApp.ConsoleApp.Services;

var log = AppState.Log = LogManager.GetCurrentClassLogger();

//const string APP_SETTINGS_CONFIGURATION_FILE = "appsettings.json";

//const string OANDA_PRACTICE_API_KEY_CONFIGURATION_KEY = "oanda:practice:ApiKey";
//const string OANDA_ACCOUNT_ID_CONFIGURATION_KEY = "oanda:account:id";
//const string OANDA_REST_API_URL_CONFIGURATION_KEY = "oanda:restApiUrl";
//const string OANDA_STREAMING_API_URL_CONFIGURATION_KEY = "oanda:streamingApiUrl";

//IConfiguration configuration = AppState.Configuration = new ConfigurationBuilder()
//    .AddJsonFile(APP_SETTINGS_CONFIGURATION_FILE, optional: false, reloadOnChange: true)
//    .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
//    .Build();


//string OANDA_REST_API_URL = configuration[OANDA_REST_API_URL_CONFIGURATION_KEY];

//string OANDA_STREAMING_API_URL = configuration[OANDA_STREAMING_API_URL_CONFIGURATION_KEY];

//string OANDA_ACCOUNT_ID = configuration[OANDA_ACCOUNT_ID_CONFIGURATION_KEY];

//Console.WriteLine($"{nameof(OANDA_REST_API_URL)} = {OANDA_REST_API_URL}");

//Console.WriteLine($"{nameof(OANDA_ACCOUNT_ID)} = {OANDA_ACCOUNT_ID}");

// API

//OandaRestApi restApi = AppState.OandaRestApi
//    = new OandaRestApi(
//    OANDA_REST_API_URL,
//    OANDA_STREAMING_API_URL,
//    configuration[OANDA_PRACTICE_API_KEY_CONFIGURATION_KEY]);

//await restApi.GetAccountDetailsAsync(OANDA_ACCOUNT_ID);
//var instruments =  await restApi.GetTradableInstrumentListAsync(OANDA_ACCOUNT_ID);
//Console.WriteLine($"{instruments.Count()} instruments found.");

//List<CandleSpecification> candleSpecifications = new List<CandleSpecification>();
//candleSpecifications.Add(new CandleSpecification("EUR_USD:S10:BM"));
//await restApi.GetLatestCandlesAsync(OANDA_ACCOUNT_ID, candleSpecifications);

//var c = await restApi.GetInstrumentCandlesForAccountAsync(OANDA_ACCOUNT_ID, "XAU_USD");

//var c2 = await restApi.GetInstrumentCandlesAsync("XAU_USD");
//var ob = await restApi.GetInstrumentOrderBookAsync("XAU_USD");
//var pb = await restApi.GetInstrumentPositionBookAsync("XAU_USD");

//var orders = await restApi.GetOrdersAsync(OANDA_ACCOUNT_ID);

//JsonSerializerOptions options = new JsonSerializerOptions()
//{
//    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
//};
//var x = new MarketOrderRequest(100, "EUR_USD", "GTD");
//var jsonText = JsonSerializer.Serialize(x, options);
//Console.WriteLine(jsonText);


//await restApi.GetPricingStreamAsync(OANDA_ACCOUNT_ID, new List<string>
//{
//    "EUR_USD", "XAU_USD"
//});


//var x = new MarketOrderRequest(1, "XAU_USD"); //OK
//var x = new LimitOrderRequest(1, "XAU_USD", 1803.855M, "GTC", 1803.820M, 1804.100M);

//JsonSerializerOptions opt = new(JsonSerializerDefaults.Web)
//{
//    WriteIndented = false,
//    //NumberHandling = JsonNumberHandling.AllowReadingFromString,
//    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
//};

//var tx = JsonSerializer.Serialize(x, opt);

//var tx2 = JsonSerializer.Serialize<object>(new NewOrder(x), opt);

//Console.WriteLine(tx);

//Console.WriteLine(tx2);

//await restApi.AddOrderAsync(OANDA_ACCOUNT_ID, x);

string rawJson;
using (StreamReader sr = new StreamReader("C:/src/github.com/ongzhixian/BasicOandaApp/dump/XAU_USD-H1-candles.json"))
{
    rawJson = sr.ReadToEnd();
}

JsonSerializerOptions opt = new JsonSerializerOptions(JsonSerializerDefaults.General)
{
    NumberHandling = JsonNumberHandling.AllowReadingFromString
};

var candleResponse = JsonSerializer.Deserialize<CandleResponse>(rawJson, opt);
IEnumerable<Ohlc>? ohlc = candleResponse.Candles.ToOhlcList(CandlestickType.Mid);

//const string TIME_COLUMN_NAME       = "t"; 
//const string OPEN_COLUMN_NAME       = "o"; 
//const string HIGH_COLUMN_NAME       = "h"; 
//const string LOW_COLUMN_NAME        = "l"; 
//const string CLOSE_COLUMN_NAME      = "c"; 
//const string VOLUME_COLUMN_NAME     = "v"; 
//const string COMPLETE_COLUMN_NAME   = "e";

//const int TIME_COLUMN_INDEX = 0;
//const int OPEN_COLUMN_INDEX = 1;
//const int HIGH_COLUMN_INDEX = 2;
//const int LOW_COLUMN_INDEX = 3;
//const int CLOSE_COLUMN_INDEX = 4;
//const int VOLUME_COLUMN_INDEX = 5;
//const int COMPLETE_COLUMN_INDEX = 6;
//const int MA_COLUMN_INDEX = 7;



//var df = new DataFrame(
//    new PrimitiveDataFrameColumn<DateTime>( TIME_COLUMN_NAME,       ohlc.Select(r => r.Time)),
//    new PrimitiveDataFrameColumn<decimal>(  OPEN_COLUMN_NAME,       ohlc.Select(r => r.O)),
//    new PrimitiveDataFrameColumn<decimal>(  HIGH_COLUMN_NAME,       ohlc.Select(r => r.H)),
//    new PrimitiveDataFrameColumn<decimal>(  LOW_COLUMN_NAME,        ohlc.Select(r => r.L)),
//    new PrimitiveDataFrameColumn<decimal>(  CLOSE_COLUMN_NAME,      ohlc.Select(r => r.C)),
//    new PrimitiveDataFrameColumn<int>(      VOLUME_COLUMN_NAME,     ohlc.Select(r => r.Volume)),
//    new PrimitiveDataFrameColumn<bool>(     COMPLETE_COLUMN_NAME,   ohlc.Select(r => r.Complete)),
//    new PrimitiveDataFrameColumn<decimal>("x", ohlc.Count())
//    );

// MathNet.Numerics.Statistics.MovingStatistics ms = new MathNet.Numerics.Statistics.MovingStatistics(8)

var df = candleResponse.Candles.ToDataFrame();
df.Columns.Add(new PrimitiveDataFrameColumn<decimal>("sma", df.Rows.Count()));

CalculationService.Sma(4, df[OhlcDataFrame.CLOSE_COLUMN_NAME], df["sma"]);


try
{
    AppState.Initialize();

    var commands = ConsoleCommandParser.GetConsoleCommands();

    if (commands.Any())
    {
        foreach (var command in commands)
        {
            command.Execute();
        }

        return;
    }

    Console.WriteLine("Do algo");

    // await InitializationSequence.RunAsync();

    //var accountList = await restApi.GetAccountListAsync();
}
catch (Exception ex)
{
    log.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}
