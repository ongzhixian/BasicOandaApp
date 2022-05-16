using System.Text.Json.Serialization;

namespace Oanda.RestApi.Models;

internal class Tag
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

}

internal class FinancingDayOfWeek
{
    [JsonPropertyName("dayOfWeek")]
    public string? DayOfWeek { get; set; }

    [JsonPropertyName("daysCharged")]
    public int DaysCharged { get; set; }
}


internal class InstrumentFinancing
{
    [JsonPropertyName("longRate ")]
    public decimal LongRate { get; set; }

    [JsonPropertyName("shortRate ")]
    public decimal ShortRate { get; set; }

    [JsonPropertyName("financingDaysOfWeek ")]
    public IList<FinancingDayOfWeek>? FinancingDaysOfWeek { get; set; }
}

internal class GuaranteedStopLossOrderLevelRestriction
{
    [JsonPropertyName("volume ")]
    public decimal Volume { get; set; }

    [JsonPropertyName("priceRange")]
    public decimal PriceRange { get; set; }

}

internal class InstrumentCommission
{
    [JsonPropertyName("commission")]
    public decimal Commission { get; set; }

    [JsonPropertyName("unitsTraded")]
    public decimal UnitsTraded { get; set; }

    [JsonPropertyName("minimumCommission")]
    public decimal MinimumCommission { get; set; }
}

internal class Instrument
{
    [JsonPropertyName("name")]
    public string Name { get; set; } // base currency and quote currency delimited by a “_”

    /// <summary>
    /// The type of the Instrument: CURRENCY / CFD / METAL 
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } 

    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; }

    /// <summary>
    /// The location of the “pip” for this instrument. 
    /// The decimal position of the pip in this Instrument’s price can be found at 10 ^ pipLocation.
    /// </summary>
    [JsonPropertyName("pipLocation")]
    public int PipLocation { get; set; }

    /// <summary>
    /// The number of decimal places that should be used to display prices for this instrument
    /// </summary>
    [JsonPropertyName("displayPrecision")]
    public int DisplayPrecision { get; set; }

    /// <summary>
    /// The amount of decimal places that may be provided when specifying the number of units traded for this instrument.
    /// </summary>
    [JsonPropertyName("tradeUnitsPrecision")]
    public int TradeUnitsPrecision { get; set; }

    [JsonPropertyName("minimumTradeSize")]
    public decimal MinimumTradeSize { get; set; }

    [JsonPropertyName("maximumTrailingStopDistance")]
    public decimal MaximumTrailingStopDistance { get; set; }

    [JsonPropertyName("minimumGuaranteedStopLossDistance")]
    public decimal MinimumGuaranteedStopLossDistance { get; set; }

    [JsonPropertyName("minimumTrailingStopDistance")]
    public decimal MinimumTrailingStopDistance { get; set; }

    [JsonPropertyName("maximumPositionSize")]
    public decimal MaximumPositionSize { get; set; }

    [JsonPropertyName("maximumOrderUnits")]
    public decimal MaximumOrderUnits { get; set; }

    [JsonPropertyName("marginRate")]
    public decimal MarginRate { get; set; }

    [JsonPropertyName("commission")]
    public InstrumentCommission Commission { get; set; }

    /// <summary>
    /// DISABLED / ALLOWED / REQUIRED
    /// </summary>
    [JsonPropertyName("guaranteedStopLossOrderMode")]
    public string? GuaranteedStopLossOrderMode { get; set; }

    [JsonPropertyName("guaranteedStopLossOrderExecutionPremium")]
    public decimal GuaranteedStopLossOrderExecutionPremium { get; set; }

    [JsonPropertyName("guaranteedStopLossOrderLevelRestriction")]
    public GuaranteedStopLossOrderLevelRestriction GuaranteedStopLossOrderLevelRestriction { get; set; }

    [JsonPropertyName("financing")]
    public InstrumentFinancing Financing { get; set; }

    [JsonPropertyName("tags")]
    public IList<Tag> Tags { get; set; }
}


internal class InstrumentList
{
    [JsonPropertyName("instruments")]
    public IEnumerable<Instrument>? Instruments { get; set; }

    [JsonPropertyName("lastTransactionID ")]
    public string LastTransactionId { get; set; }
}

internal class OrderBookBucket
{
    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("longCountPercent")]
    public decimal LongCountPercent { get; set; }

    [JsonPropertyName("shortCountPercent")]
    public decimal ShortCountPercent { get; set; }
}

internal class OrderBook
{
    [JsonPropertyName("instrument")]
    public string Instrument { get; set; }

    [JsonPropertyName("time")]
    public DateTime Time { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("bucketWidth")]
    public decimal BucketWidth { get; set; }

    [JsonPropertyName("buckets")]
    public IList<OrderBookBucket> Buckets { get; set; }
}

internal class InstrumentOrderBook
{
    [JsonPropertyName("orderBook")]
    public OrderBook OrderBook { get; set; }

}

internal class PositionBookBucket
{
    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("longCountPercent")]
    public decimal LongCountPercent { get; set; }

    [JsonPropertyName("shortCountPercent")]
    public decimal ShortCountPercent { get; set; }
}

internal class PositionBook
{
    [JsonPropertyName("instrument")]
    public string Instrument { get; set; }

    [JsonPropertyName("time")]
    public DateTime Time { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("bucketWidth")]
    public decimal BucketWidth { get; set; }

    [JsonPropertyName("buckets")]
    public IList<PositionBookBucket> Buckets { get; set; }
}

internal class InstrumentPositionBook
{
    [JsonPropertyName("positionBook")]
    public PositionBook PositionBook { get; set; }

}