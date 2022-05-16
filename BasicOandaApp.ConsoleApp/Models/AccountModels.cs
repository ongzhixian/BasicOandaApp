using System.Text.Json.Serialization;

namespace Oanda.RestApi.Models;


internal class AccountProperties
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("mt4AccountID")]
    public int MT4AccountId { get; set; }

    [JsonPropertyName("tags")]
    public IList<string> Tag { get; set; }
}


internal class AccountList
{
    [JsonPropertyName("accounts")]
    public IEnumerable<AccountProperties>? Accounts { get; set; }
}
