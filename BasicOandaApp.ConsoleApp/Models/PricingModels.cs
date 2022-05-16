using System.Text.Json.Serialization;

namespace Oanda.RestApi.Models;


internal class PricingHeartbeat
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "HEARTBEAT";

    [JsonPropertyName("time")]
    public DateTime Time { get; set; }
}


//internal class ClientPrice
//{
//    [JsonPropertyName("type")]
//    public string Type { get; set; } = "PRICE";

//    [JsonPropertyName("instrument")]
//    public string Instrument { get; set; }

//    [JsonPropertyName("time")]
//    public DateTime Time { get; set; }

//    [JsonPropertyName("tradeable")]
//    public bool Tradeable { get; set; }

//    [JsonPropertyName("bids")]
//    public IList<PriceBucket> Bids { get; set; }

//    [JsonPropertyName("asks")]
//    public IList<PriceBucket> Asks { get; set; }

//    [JsonPropertyName("closeoutBid")]
//    public decimal CloseoutBid { get; set; }

//    [JsonPropertyName("closeoutAsk")]
//    public decimal CloseoutAsk { get; set; }
//}
