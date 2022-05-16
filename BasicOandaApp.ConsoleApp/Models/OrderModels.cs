using System.Text.Json.Serialization;

namespace Oanda.RestApi.Models;

internal class ClientExtension
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("tag")]
    public string Tag { get; set; }

    [JsonPropertyName("comment")]
    public string Comment { get; set; }

}

internal class Order
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("createTime")]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Current state of order: PENDING / FILLED / TRIGGERED / CANCELLED
    /// </summary>
    [JsonPropertyName("state")]
    public string State { get; set; }

    /// <summary>
    /// The client extensions of the Order. Do not set, modify, or delete clientExtensions if your account is associated with MT4.
    /// </summary>
    [JsonPropertyName("clientExtensions")]
    public ClientExtension ClientExtensions { get; set; }
}


internal class OrderList
{
    [JsonPropertyName("orders")]
    public IEnumerable<Order>? Orders { get; set; }

    [JsonPropertyName("lastTransactionID")]
    public string LastTransactionId { get; set; }

}



internal class TakeProfitDetails
{
    public TakeProfitDetails(decimal price)
    {
        Price = price;
    }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("timeInForce")]
    public string TimeInForce { get; set; } = "GTC";

    [JsonPropertyName("gtdTime")]
    public DateTime? GTDTime { get; set; }

    [JsonPropertyName("clientExtensions")]
    public ClientExtension? ClientExtensions { get; set; }
}

internal class StopLossDetails
{
    public StopLossDetails(decimal price, string timeInForce = "GTC")
    {
        Price = price;
        TimeInForce = timeInForce;
    }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("distance")]
    public decimal? Distance { get; set; }

    [JsonPropertyName("timeInForce")]
    public string TimeInForce { get; set; } = "GTC";

    [JsonPropertyName("gtdTime")]
    public DateTime? GTDTime { get; set; }

    [JsonPropertyName("clientExtensions")]
    public ClientExtension? ClientExtensions { get; set; }
}

internal class GuaranteedStopLossDetails
{
    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("distance")]
    public decimal distance { get; set; }

    [JsonPropertyName("timeInForce")]
    public string TimeInForce { get; set; } = "GTC";

    [JsonPropertyName("gtdTime")]
    public DateTime GTDTime { get; set; }

    [JsonPropertyName("clientExtensions")]
    public ClientExtension ClientExtensions { get; set; }
}

internal class TrailingStopLossDetails
{
    [JsonPropertyName("distance")]
    public decimal distance { get; set; }

    [JsonPropertyName("timeInForce")]
    public string TimeInForce { get; set; } = "GTC";

    [JsonPropertyName("gtdTime")]
    public DateTime GTDTime { get; set; }

    [JsonPropertyName("clientExtensions")]
    public ClientExtension ClientExtensions { get; set; }
}

internal class OrderRequest
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "MARKET";

    [JsonPropertyName("instrument")]
    public string Instrument { get; set; }

    [JsonPropertyName("units")]
    public decimal Units { get; set; }

    // price: LimitOrderRequest / StopOrderRequest

    // priceBound: MarketOrderRequest / StopOrderRequest

    [JsonPropertyName("timeInForce")]
    public string TimeInForce { get; set; } = "FOK";

    // gtdTime: LimitOrderRequest / StopOrderRequest

    [JsonPropertyName("positionFill")]
    public string PositionFill { get; set; } = "DEFAULT";

    // triggerCondition: LimitOrderRequest / StopOrderRequest

    // MarketOrderRequest / LimitOrderRequest / StopOrderRequest

    [JsonPropertyName("clientExtensions")]
    public ClientExtension? ClientExtensions { get; set; }

    [JsonPropertyName("takeProfitOnFill")]
    public TakeProfitDetails? TakeProfitOnFill { get; set; }

    [JsonPropertyName("stopLossOnFill")]
    public StopLossDetails? StopLossOnFill { get; set; }

    [JsonPropertyName("guaranteedStopLossOnFill")]
    public GuaranteedStopLossDetails? GuaranteedStopLossOnFill { get; set; }

    [JsonPropertyName("trailingStopLossOnFill")]
    public TrailingStopLossDetails? TrailingStopLossOnFill { get; set; }

    [JsonPropertyName("tradeClientExtensions")]
    public ClientExtension? TradeClientExtensions { get; set; }
}

internal class MarketOrderRequest : OrderRequest
{
    public MarketOrderRequest()
    {
        this.Type = "MARKET";
        this.TimeInForce = "FOK";
    }

    public MarketOrderRequest(decimal units, string instrument, string timeInForce = "FOK") : this()
    {
        this.Units = units;
        this.Instrument = instrument;
        this.TimeInForce = timeInForce;
    }

    [JsonPropertyName("priceBound")]
    public decimal? PriceBound { get; set; }
}

internal class LimitOrderRequest : OrderRequest
{
    public LimitOrderRequest()
    {
        this.Type = "LIMIT";
        this.TimeInForce = "GTC";
    }

    public LimitOrderRequest(decimal units, string instrument, decimal price, 
        string timeInForce = "GTC", decimal? stopLoss = null, decimal? takeProfit = null) : this()
    {
        Units = units;
        Instrument = instrument;
        Price = price;
        TimeInForce = timeInForce;

        if (stopLoss.HasValue)
        {
            this.StopLossOnFill = new StopLossDetails(stopLoss.Value);
        }

        if (takeProfit.HasValue)
        {
            this.TakeProfitOnFill = new TakeProfitDetails(takeProfit.Value);
        }
    }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }


    [JsonPropertyName("gtdTime")]
    public DateTime? GTDTime { get; set; }


    [JsonPropertyName("triggerCondition")]
    public string TriggerCondition { get; set; } = "DEFAULT";

}

internal class StopOrderRequest : OrderRequest
{
    public StopOrderRequest()
    {
        this.Type = "STOP";
        this.TimeInForce = "GTC";
    }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("gtdTime")]
    public DateTime? GTDTime { get; set; }

    [JsonPropertyName("triggerCondition")]
    public string TriggerCondition { get; set; } = "DEFAULT";
}

internal class MarketIfTouchedOrderRequest : OrderRequest
{
    public MarketIfTouchedOrderRequest()
    {
        this.Type = "MARKET_IF_TOUCHED";
        this.TimeInForce = "GTC";
    }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("gtdTime")]
    public DateTime? GTDTime { get; set; }

    [JsonPropertyName("triggerCondition")]
    public string TriggerCondition { get; set; } = "DEFAULT";
}

internal class NewOrder
{
    // In order to have polymorphic serialization, we need to declare this as object
    // By default, System.Text.Json only serialize immediate class and not derived class/fields
    // See: https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-polymorphism
    // Something to try: How about generics?

    [JsonPropertyName("order")]
    public object Order { get; set; }

    public NewOrder(OrderRequest order)
    {
        Order = order;
    }
}

internal class Transaction
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("time")]
    public DateTime time { get; set; }

    [JsonPropertyName("userID")]
    public int UserId { get; set; }

    [JsonPropertyName("accountID")]
    public string AccountId { get; set; }

    [JsonPropertyName("batchID")]
    public string BatchId { get; set; }

    [JsonPropertyName("requestID")]
    public string RequestId { get; set; }
}

internal class ConversionFactor
{
    [JsonPropertyName("factor")]
    public decimal Factor { get; set; }
}

internal class HomeConversionFactors
{
    [JsonPropertyName("gainQuoteHome")]
    public ConversionFactor GainQuoteHome { get; set; }

    [JsonPropertyName("lossQuoteHome")]
    public ConversionFactor LossQuoteHome { get; set; }

    [JsonPropertyName("gainBaseHome")]
    public ConversionFactor GainBaseHome { get; set; }

    [JsonPropertyName("lossBaseHome")]
    public ConversionFactor LossBaseHome { get; set; }
}

internal class PriceBucket
{
    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("liquidity")]
    public decimal Liquidity { get; set; }
}

internal class ClientPrice
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("instrument")]
    public string Instrument { get; set; }

    [JsonPropertyName("time")]
    public DateTime Time { get; set; }

    [JsonPropertyName("tradeable")]
    public bool Tradeable { get; set; }

    [JsonPropertyName("bids")]
    public IList<PriceBucket> Bids { get; set; }

    [JsonPropertyName("asks")]
    public IList<PriceBucket> Asks { get; set; }

    [JsonPropertyName("closeoutBid")]
    public decimal CloseoutBid { get; set; }

    [JsonPropertyName("closeoutAsk")]
    public decimal CloseoutAsk { get; set; }
}

internal class TradeOpen
{
    [JsonPropertyName("tradeID")]
    public int TradeId { get; set; }

    [JsonPropertyName("units")]
    public decimal Units { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("guaranteedExecutionFee")]
    public string GuaranteedExecutionFee { get; set; }

    [JsonPropertyName("quoteGuaranteedExecutionFee")]
    public decimal QuoteGuaranteedExecutionFee { get; set; }

    [JsonPropertyName("clientExtensions")]
    public ClientExtension? ClientExtensions { get; set; }

    [JsonPropertyName("halfSpreadCost")]
    public string HalfSpreadCost { get; set; }

    [JsonPropertyName("initialMarginRequired")]
    public string InitialMarginRequired { get; set; }
}

internal class TradeReduce
{
    [JsonPropertyName("tradeID")]
    public int TradeId { get; set; }

    [JsonPropertyName("units")]
    public decimal Units { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }


    [JsonPropertyName("realizedPL")]
    public string RealizedPL { get; set; }

    [JsonPropertyName("financing")]
    public string Financing { get; set; }

    [JsonPropertyName("baseFinancing")]
    public decimal BaseFinancing { get; set; }

    [JsonPropertyName("quoteFinancing")]
    public decimal QuoteFinancing { get; set; }

    [JsonPropertyName("financingRate")]
    public decimal FinancingRate { get; set; }

    [JsonPropertyName("guaranteedExecutionFee")]
    public string GuaranteedExecutionFee { get; set; }

    [JsonPropertyName("quoteGuaranteedExecutionFee")]
    public decimal QuoteGuaranteedExecutionFee { get; set; }
    
    [JsonPropertyName("halfSpreadCost")]
    public string HalfSpreadCost { get; set; }
}

internal class OrderFillTransaction
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("time")]
    public DateTime time { get; set; }

    [JsonPropertyName("userID")]
    public int UserId { get; set; }

    [JsonPropertyName("accountID")]
    public string AccountId { get; set; }

    [JsonPropertyName("batchID")]
    public string BatchId { get; set; }

    [JsonPropertyName("requestID")]
    public string RequestId { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("orderID")]
    public string OrderID { get; set; }

    [JsonPropertyName("clientOrderID")]
    public string ClientOrderID { get; set; }

    [JsonPropertyName("instrument")]
    public string instrument { get; set; }

    [JsonPropertyName("units")]
    public decimal units { get; set; }

    [JsonPropertyName("homeConversionFactors")]
    public HomeConversionFactors HomeConversionFactors { get; set; }

    [JsonPropertyName("fullVWAP")]
    public decimal FullVWAP { get; set; }

    [JsonPropertyName("fullPrice")]
    public ClientPrice FullPrice { get; set; }

    [JsonPropertyName("reason")]
    public string reason { get; set; }

    [JsonPropertyName("pl")]
    public string PL { get; set; }

    [JsonPropertyName("quotePL")]
    public decimal QuotePL { get; set; }

    [JsonPropertyName("financing")]
    public string Financing { get; set; }

    [JsonPropertyName("baseFinancing")]
    public decimal BaseFinancing { get; set; }

    [JsonPropertyName("quoteFinancing")]
    public decimal QuoteFinancing { get; set; }

    [JsonPropertyName("commission")]
    public string Commission { get; set; }

    [JsonPropertyName("guaranteedExecutionFee")]
    public string GuaranteedExecutionFee { get; set; }

    [JsonPropertyName("quoteGuaranteedExecutionFee")]
    public decimal QuoteGuaranteedExecutionFee { get; set; }

    [JsonPropertyName("accountBalance")]
    public string AccountBalance { get; set; }

    [JsonPropertyName("tradeOpened")]
    public TradeOpen TradeOpened { get; set; }

    [JsonPropertyName("tradesClosed")]
    public IList<TradeReduce> TradesClosed { get; set; }

    [JsonPropertyName("tradeReduced")]
    public TradeReduce TradeReduced { get; set; }

    [JsonPropertyName("halfSpreadCost")]
    public string HalfSpreadCost { get; set; }
}

internal class OrderCancelTransaction
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("time")]
    public DateTime Time { get; set; }

    [JsonPropertyName("userID")]
    public int UserId { get; set; }

    [JsonPropertyName("accountID")]
    public string AccountId { get; set; }

    [JsonPropertyName("batchID")]
    public string BatchID { get; set; }

    [JsonPropertyName("requestID")]
    public string RequestID { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }


    [JsonPropertyName("orderID")]
    public int OrderID { get; set; }

    [JsonPropertyName("clientOrderID")]
    public int ClientOrderID { get; set; }

    [JsonPropertyName("reason")]
    public string Reason { get; set; }

    [JsonPropertyName("replacedByOrderID")]
    public int ReplacedByOrderID { get; set; }
}


internal class OrderCreate
{
    [JsonPropertyName("orderCreateTransaction")]
    public Transaction OrderCreateTransaction { get; set; }

    [JsonPropertyName("orderFillTransaction")]
    public OrderFillTransaction OrderFillTransaction { get; set; }

    [JsonPropertyName("orderCancelTransaction")]
    public OrderCancelTransaction OrderCancelTransaction { get; set; }

    [JsonPropertyName("orderReissueTransaction")]
    public Transaction OrderReissueTransaction { get; set; }

    [JsonPropertyName("orderReissueRejectTransaction")]
    public Transaction OrderReissueRejectTransaction { get; set; }

    [JsonPropertyName("relatedTransactionIDs")]
    public IList<string> RelatedTransactionIDs { get; set; }

    [JsonPropertyName("lastTransactionID")]
    public string LastTransactionID { get; set; }
}