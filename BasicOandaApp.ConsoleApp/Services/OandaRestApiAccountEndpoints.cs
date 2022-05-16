using Oanda.RestApi.Models;
using System.Text.Json;

namespace Oanda.RestApi.Services;

internal partial class OandaRestApi
{
    public async Task<IEnumerable<AccountProperties>?> GetAccountListAsync()
    {
        const string url = "/v3/accounts";

        using HttpResponseMessage? httpResponse = await httpClient.GetAsync(url);

        httpResponse.EnsureSuccessStatusCode();

        using Stream? strm = await httpResponse.Content.ReadAsStreamAsync();

        AccountList? result =
            await JsonSerializer.DeserializeAsync<AccountList>(strm, jsonSerializerOptions);

        if (result == null)
        {
            return new List<AccountProperties>();
        }

        return result.Accounts;
    }

    public async Task GetAccountDetailsAsync(string accountId)
    {
        string url = $"/v3/accounts/{accountId}";

        using HttpResponseMessage? httpResponse = await httpClient.GetAsync(url);

        httpResponse.EnsureSuccessStatusCode();

        using Stream? strm = await httpResponse.Content.ReadAsStreamAsync();

        await DumpAsync(strm);

        // TODO: 

        //AccountList? result =
        //    await JsonSerializer.DeserializeAsync<AccountList>(strm, jsonSerializerOptions);

        //if (result == null)
        //{
        //    return new List<AccountProperties>();
        //}

        //return result.Accounts;
    }

    public async Task<IEnumerable<Instrument>> GetTradableInstrumentListAsync(string accountId)
    {
        string url = $"/v3/accounts/{accountId}/instruments";

        using HttpResponseMessage? httpResponse = await httpClient.GetAsync(url);

        httpResponse.EnsureSuccessStatusCode();

        using Stream? strm = await httpResponse.Content.ReadAsStreamAsync();

        InstrumentList? result =
            await JsonSerializer.DeserializeAsync<InstrumentList>(strm, jsonSerializerOptions);

        if (result == null || result.Instruments == null)
        {
            return new List<Instrument>();
        }

        return result.Instruments;
    }
}
