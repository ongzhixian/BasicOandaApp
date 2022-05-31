using BasicOandaApp.CandlestockSimulator.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using BasicOandaApp.CandlestockSimulator.Extensions;
using System.Globalization;

string[] arguments = Environment.GetCommandLineArgs(); 

if (arguments.Length < 1)
{
    Console.WriteLine("No data file specified.");
    return;
}

string dataFileName = arguments[1]; // args[0] = exec; args[1] are the real options

IEnumerable<LabelledOhlc>? labelledOhlc = GetCandles(dataFileName);

if (labelledOhlc == null)
{
    Console.Error.WriteLine("Error reading data file.");
    return;
}

var pip = (decimal)Math.Pow(10, -2);

Console.WriteLine(pip);

decimal? activityLevel = null;
int prevVolume = 0;

foreach (var item in labelledOhlc.Take(20))
{
    var hlDiscrepancy = (item.H - item.O) / pip;
    var clDiscrepancy = (item.C - item.L) / pip;
    var wickLength = (item.H - item.L) / pip;
    var candleLength = Math.Abs(item.O - item.C) / pip;
    var candleWickRatio = candleLength / wickLength;

    // If open is less than close, "GRN" (increase)
    // If open is more than close, "RED" (decrease)
    var cdlType = item.O < item.C ? "Grn" : "Red";

    // {hlDiscrepancy,6} : {clDiscrepancy, 6} : {wickLength, 6}, {candleLength, 6}, {candleWickRatio, 6:F2}, 
    
    if (prevVolume == 0)
        activityLevel = null;
    else
        activityLevel = (item.Volume - prevVolume) / (decimal)prevVolume;

    prevVolume = item.Volume;

    Console.WriteLine($"{item}, {cdlType}, {activityLevel,5:F2}");
}

IEnumerable<LabelledOhlc>? GetCandles(string dataFileName)
{
    JsonSerializerOptions opt = new JsonSerializerOptions(JsonSerializerDefaults.General)
    {
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };

    string rawJson;

    using StreamReader sr = new(dataFileName);

    rawJson = sr.ReadToEnd();

    var candleResponse = JsonSerializer.Deserialize<OandaCandleResponse>(rawJson, opt);

    Console.WriteLine(candleResponse?.Candles?.Count);

    return candleResponse?.Candles?.ToLabelledOhlcList();
}
