using BasicOandaApp.ConsoleApp;
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

    public async Task<Account> GetAccountDetailsAsync(string accountId)
    {
        string url = $"/v3/accounts/{accountId}";

        using HttpResponseMessage? httpResponse = await httpClient.GetAsync(url);

        httpResponse.EnsureSuccessStatusCode();

        using Stream? strm = await httpResponse.Content.ReadAsStreamAsync();

        // TODO: await DumpAsync(strm);

        Account? result = await JsonSerializer.DeserializeAsync<Account>(strm, jsonSerializerOptions);

        log.Info("{information} {result}", "Account information", result == null ? "MISSING" : "OK");

        return result ?? throw new NullReferenceException();
    }

    public async Task<IEnumerable<Instrument>> GetTradableInstrumentListAsync(string accountId)
    {
        string url = $"/v3/accounts/{accountId}/instruments";

        using HttpResponseMessage? httpResponse = await httpClient.GetAsync(url);

        httpResponse.EnsureSuccessStatusCode();

        using Stream? strm = await httpResponse.Content.ReadAsStreamAsync();

        InstrumentList? result =
            await JsonSerializer.DeserializeAsync<InstrumentList>(strm, jsonSerializerOptions);

        log.Info("{information} {result}", "Tradable instruments information", result?.Instruments == null ? "MISSING" : "OK");

        return result?.Instruments ?? throw new NullReferenceException();
    }

    public async Task<string> DumpTradableInstrumentListAsync()
    {
        string accountId = AppState.OandaAccountId;

        string url = $"/v3/accounts/{accountId}/instruments";

        using HttpResponseMessage? httpResponse = await httpClient.GetAsync(url);

        httpResponse.EnsureSuccessStatusCode();

        using Stream? strm = await httpResponse.Content.ReadAsStreamAsync();

        using StreamReader streamReader = new StreamReader(strm);
        
        return await streamReader.ReadToEndAsync();
    }
}
