using System.Text.Json.Serialization;

namespace Oanda.RestApi.Models;

internal class CandlestickData
{
    /// <summary>
    /// The first (open) price in the time-range represented by the candlestick
    /// </summary>
    [JsonPropertyName("o")]
    public decimal O { get; set; }

    /// <summary>
    /// The highest price in the time-range represented by the candlestick.
    /// </summary>
    [JsonPropertyName("h")]
    public decimal H { get; set; }

    /// <summary>
    /// The lowest price in the time-range represented by the candlestick.
    /// </summary>
    [JsonPropertyName("l")]
    public decimal L { get; set; }

    /// <summary>
    /// The last (closing) price in the time-range represented by the candlestick.
    /// </summary>
    [JsonPropertyName("c")]
    public decimal C { get; set; }

}

internal class Candlestick
{
    [JsonPropertyName("time")]
    public DateTime Time { get; set; }

    [JsonPropertyName("bid")]
    public CandlestickData Bid { get; set; }

    [JsonPropertyName("ask")]
    public CandlestickData Ask { get; set; }

    [JsonPropertyName("mid")]
    public CandlestickData Mid { get; set; }

    [JsonPropertyName("volume")]
    public int Volume { get; set; }

    [JsonPropertyName("complete")]
    public bool Complete { get; set; }
}

internal class CandleResponse
{
    [JsonPropertyName("instrument")]
    public string Instrument { get; set; }

    [JsonPropertyName("granularity")]
    public string Granularity { get; set; }

    [JsonPropertyName("candles")]
    public IList<Candlestick> Candles { get; set; }

}

internal class LatestCandleList
{
    [JsonPropertyName("latestCandles")]
    public IEnumerable<CandleResponse>? CandleResponseList { get; set; }
}


/// <summary>
/// A string containing the following, all delimited by “:” characters: 
/// 1) InstrumentName 
/// 2) CandlestickGranularity 
/// 3) PricingComponent 
/// e.g. EUR_USD:S10:BM
/// </summary>
internal class CandleSpecification
{
    /// <summary>
    /// A string containing the base currency and quote currency delimited by a "_".
    /// </summary>
    public string InstrumentName { get; set; }

    /// <summary>
    /// Granularity of candlestick:
    /// S5/10/15/30     n-second candlesticks, minute alignment
    /// M1              1 minute candlesticks, minute alignment
    /// M2/4/5/10/15/30 n-minute candlesticks, hour alignment
    /// H1              1 hour candlesticks, hour alignment
    /// H2/3/4/6/8/12   n-hour candlesticks, day alignment
    /// D 	            1 day candlesticks, day alignment
    /// W 	            1 week candlesticks, aligned to start of week
    /// M               1 month candlesticks, aligned to first day of the month
    /// </summary>
    public string CandlestickGranularity { get; set; }

    /// <summary>
    /// Can contain any combination of the characters:
    /// "M" (midpoint candles)
    /// "B" (bid candles) and 
    /// "A" (ask candles).
    /// </summary>
    public string PricingComponent { get; set; }

    public CandleSpecification(string instrumentName, string candlestickGranularity, string pricingComponent)
    {
        this.InstrumentName = instrumentName;
        this.CandlestickGranularity = candlestickGranularity;
        this.PricingComponent = pricingComponent;
    }

    public CandleSpecification(string specification)
    {
        var parts = specification.Split(':', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 3)
        {
            throw new ArgumentException(nameof(specification));
        }

        this.InstrumentName = parts[0].Trim();
        this.CandlestickGranularity = parts[1].Trim();
        this.PricingComponent = parts[2].Trim();
    }

    public override string ToString()
    {
        return $"{InstrumentName}:{CandlestickGranularity}:{PricingComponent}";
    }
}

internal static class CandleSpecificationListExtensions
{
    public static string ToCsv(this List<CandleSpecification> candleSpecifications)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        foreach (var item in candleSpecifications)
        {
            sb.Append(item.ToString());
            sb.Append(',');
        }

        // Remove trailing comma
        return sb.ToString().Remove(sb.Length - 1, 1);
    }
}