using BasicOandaApp.ConsoleApp;
using Oanda.RestApi.Models;
using System.Text;
using System.Text.Json;

namespace Oanda.RestApi.Services;

internal partial class OandaRestApi
{
    public async Task<IEnumerable<CandleResponse>?> GetLatestCandlesAsync(string accountId, List<CandleSpecification> candleSpecificationList)
    {
        string url = $"/v3/accounts/{accountId}/candles/latest?candleSpecifications={candleSpecificationList.ToCsv()}";

        using HttpResponseMessage? httpResponse = await httpClient.GetAsync(url);

        httpResponse.EnsureSuccessStatusCode();

        using Stream? strm = await httpResponse.Content.ReadAsStreamAsync();

        //await DumpAsync(strm);

        LatestCandleList? result =
            await JsonSerializer.DeserializeAsync<LatestCandleList>(strm, jsonSerializerOptions);

        if (result == null || result.CandleResponseList == null)
        {
            return new List<CandleResponse>();
        }

        return result.CandleResponseList;
    }

    public async Task<IList<Candlestick>> GetInstrumentCandlesForAccountAsync(string accountId, string instrument)
    {
        string url = $"/v3/accounts/{accountId}/instruments/{instrument}/candles";

        using HttpResponseMessage? httpResponse = await httpClient.GetAsync(url);

        httpResponse.EnsureSuccessStatusCode();

        using Stream? strm = await httpResponse.Content.ReadAsStreamAsync();

        CandleResponse? result =
            await JsonSerializer.DeserializeAsync<CandleResponse>(strm, jsonSerializerOptions);

        if (result == null || result.Candles == null)
        {
            return new List<Candlestick>();
        }

        return result.Candles;
    }

    public async Task<string> DumpInstrumentCandlesForAccountAsync(string instrument, string granularity)
    {
        string accountId = AppState.OandaAccountId;

        string url = $"/v3/accounts/{accountId}/instruments/{instrument}/candles?granularity={granularity}";

        using HttpResponseMessage? httpResponse = await httpClient.GetAsync(url);

        httpResponse.EnsureSuccessStatusCode();

        using Stream? strm = await httpResponse.Content.ReadAsStreamAsync();

        using StreamReader streamReader = new StreamReader(strm);

        return await streamReader.ReadToEndAsync();
    }

    public async Task GetPricingStreamAsync(string accountId, IList<string> instrumentList)
    {
        string instrumentsCsv = ToCsv(instrumentList);

        string url = $"/v3/accounts/{accountId}/pricing/stream?instruments={instrumentsCsv}";

        using Stream? strm = await streamingHttpClient.GetStreamAsync(url);

        byte[] readBuffer = new byte[1024];

        while (true)
        {
            //var s = await JsonSerializer.DeserializeAsync<>(strm);

            int bytesRead = await strm.ReadAsync(readBuffer, 0, 1024);

            var jsonText = Encoding.UTF8.GetString(readBuffer, 0, bytesRead);

            var entries = jsonText.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine($"{entries.Count()} entries received");
            Console.WriteLine(jsonText);

            foreach (var item in entries)
            {
                if (item.StartsWith("{\"type\":\"PRICE\","))
                {
                    var clientPrice = JsonSerializer.Deserialize<ClientPrice>(item, jsonSerializerOptions);
                    Console.WriteLine("clientPrice parsed OK");
                }
                else
                {
                    // {"type":"HEARTBEAT"
                    var pricingHeartbeat = JsonSerializer.Deserialize<PricingHeartbeat>(item, jsonSerializerOptions);
                    Console.WriteLine("pricingHeartbeat parsed OK");
                }
            }


            //Console.Write(System.Text.Encoding.UTF8.GetString(readBuffer, 0, bytesRead));
        }
        

        //CandleResponse? result =
        //    await JsonSerializer.DeserializeAsync<CandleResponse>(strm, jsonSerializerOptions);

        //if (result == null || result.Candles == null)
        //{
        //    return new List<Candlestick>();
        //}

        //return result.Candles;
    }

    private static string ToCsv(IList<string> instrumentList)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        foreach (var item in instrumentList)
        {
            sb.Append($"{item}");
            sb.Append(',');
        }

        sb.Remove(sb.Length - 1, 1);
        
        return sb.ToString();
    }
}
