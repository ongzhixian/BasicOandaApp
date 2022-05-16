using Microsoft.Extensions.Configuration;
using Oanda.RestApi.Models;
using Oanda.RestApi.Services;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

Console.WriteLine("Hello, World!");

const string APP_SETTINGS_CONFIGURATION_FILE = "appsettings.json";

const string OANDA_PRACTICE_API_KEY_CONFIGURATION_KEY = "oanda:practice:ApiKey";
const string OANDA_ACCOUNT_ID_CONFIGURATION_KEY = "oanda:account:id";
const string OANDA_REST_API_URL_CONFIGURATION_KEY = "oanda:restApiUrl";
const string OANDA_STREAMING_API_URL_CONFIGURATION_KEY = "oanda:streamingApiUrl";

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile(APP_SETTINGS_CONFIGURATION_FILE, optional: false, reloadOnChange: true)
    .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
    .Build();

string OANDA_REST_API_URL = configuration[OANDA_REST_API_URL_CONFIGURATION_KEY];

string OANDA_STREAMING_API_URL = configuration[OANDA_STREAMING_API_URL_CONFIGURATION_KEY];

string OANDA_ACCOUNT_ID = configuration[OANDA_ACCOUNT_ID_CONFIGURATION_KEY];

Console.WriteLine($"{nameof(OANDA_REST_API_URL)} = {OANDA_REST_API_URL}");

Console.WriteLine($"{nameof(OANDA_ACCOUNT_ID)} = {OANDA_ACCOUNT_ID}");

// API

OandaRestApi restApi = new OandaRestApi(
    OANDA_REST_API_URL,
    OANDA_STREAMING_API_URL,
    configuration[OANDA_PRACTICE_API_KEY_CONFIGURATION_KEY]);

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
var x = new LimitOrderRequest(1, "XAU_USD", 1803.855M, "GTC", 1803.820M, 1804.100M);

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

await restApi.AddOrderAsync(OANDA_ACCOUNT_ID, x);

Console.WriteLine("Press <ENTER> to exit.");
Console.ReadLine();
