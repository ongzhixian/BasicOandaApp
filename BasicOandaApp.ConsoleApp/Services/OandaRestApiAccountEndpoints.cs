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

        log.Info("Account information {result}", result == null ? "MISSING" : "OK");

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

        log.Info("Tradable instruments information {result}", result?.Instruments == null ? "MISSING" : "OK");

        return result?.Instruments ?? throw new NullReferenceException();
    }

    public async Task DumpTradableInstrumentListAsync(string accountId, string filePath)
    {
        string? fullFilePath = Path.GetFullPath(filePath);

        Console.WriteLine(fullFilePath);

        string? fullFileDirectoryPath = Path.GetDirectoryName(fullFilePath);

        if (fullFileDirectoryPath != null && !Directory.Exists(fullFileDirectoryPath))
        {
            Directory.CreateDirectory(fullFileDirectoryPath);
        }

        string url = $"/v3/accounts/{accountId}/instruments";

        using HttpResponseMessage? httpResponse = await httpClient.GetAsync(url);

        httpResponse.EnsureSuccessStatusCode();

        using Stream? strm = await httpResponse.Content.ReadAsStreamAsync();

        using StreamReader streamReader = new StreamReader(strm);

        using StreamWriter sw = new StreamWriter(fullFileDirectoryPath, false);

        sw.AutoFlush = true;

        await sw.WriteAsync(await streamReader.ReadToEndAsync());
        
    }
}
