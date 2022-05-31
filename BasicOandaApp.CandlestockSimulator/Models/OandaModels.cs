using System.Text.Json.Serialization;

namespace BasicOandaApp.CandlestockSimulator.Models;

internal class OandaCandlestickData
{
    [JsonPropertyName("o")]
    public decimal O { get; set; }

    [JsonPropertyName("h")]
    public decimal H { get; set; }

    [JsonPropertyName("l")]
    public decimal L { get; set; }

    [JsonPropertyName("c")]
    public decimal C { get; set; }

}

internal class OandaCandlestick
{
    [JsonPropertyName("time")]
    public DateTime Time { get; set; }

    [JsonPropertyName("bid")]
    public OandaCandlestickData? Bid { get; set; }

    [JsonPropertyName("ask")]
    public OandaCandlestickData? Ask { get; set; }

    [JsonPropertyName("mid")]
    public OandaCandlestickData? Mid { get; set; }

    [JsonPropertyName("volume")]
    public int Volume { get; set; }

    [JsonPropertyName("complete")]
    public bool Complete { get; set; }
}

internal class OandaCandleResponse
{
    [JsonPropertyName("instrument")]
    public string? Instrument { get; set; }

    [JsonPropertyName("granularity")]
    public string? Granularity { get; set; }

    [JsonPropertyName("candles")]
    public IList<OandaCandlestick>? Candles { get; set; }

}