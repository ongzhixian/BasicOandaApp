using System.Text.Json.Serialization;

namespace BasicOandaApp.CandlestockSimulator.Models;

internal class Ohlc
{
    [JsonPropertyName("t")]
    public DateTime Time { get; set; }

    [JsonPropertyName("o")]
    public decimal O { get; set; }

    [JsonPropertyName("h")]
    public decimal H { get; set; }

    [JsonPropertyName("l")]
    public decimal L { get; set; }

    [JsonPropertyName("c")]
    public decimal C { get; set; }

    [JsonPropertyName("volume")]
    public int Volume { get; set; }

    [JsonPropertyName("complete")]
    public bool Complete { get; set; }

    public override string ToString()
    {
        return $"T: {Time:MMdd-HH}, O: {O}, H: {H}, L: {L}, C: {C}, V:{Volume,6}";
    }
}

internal class LabelledOhlc : Ohlc
{
    public string Label { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"T: {Time:MMdd-HH}, O: {O}, H: {H}, L: {L}, C: {C}, V:{Volume,6}, Label: {Label}";
    }
}
