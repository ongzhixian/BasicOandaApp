using Oanda.RestApi.Models;
using System.Text.Json;

namespace Oanda.RestApi.Services;

internal partial class OandaRestApi
{
    public async Task<IList<Candlestick>> GetInstrumentCandlesAsync(string instrument)
    {
        string url = $"/v3/instruments/{instrument}/candles";

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

    public async Task<OrderBook> GetInstrumentOrderBookAsync(string instrument)
    {
        string url = $"/v3/instruments/{instrument}/orderBook";

        using HttpResponseMessage? httpResponse = await httpClient.GetAsync(url);

        httpResponse.EnsureSuccessStatusCode();

        using Stream? strm = await httpResponse.Content.ReadAsStreamAsync();

        //await DumpAsync(strm);

        InstrumentOrderBook? result =
            await JsonSerializer.DeserializeAsync<InstrumentOrderBook>(strm, jsonSerializerOptions);

        if (result == null || result.OrderBook == null)
        {
            return new OrderBook();
        }

        return result.OrderBook;
    }

    public async Task<PositionBook> GetInstrumentPositionBookAsync(string instrument)
    {
        string url = $"/v3/instruments/{instrument}/positionBook";

        using HttpResponseMessage? httpResponse = await httpClient.GetAsync(url);

        httpResponse.EnsureSuccessStatusCode();

        using Stream? strm = await httpResponse.Content.ReadAsStreamAsync();

        //await DumpAsync(strm);

        InstrumentPositionBook? result =
            await JsonSerializer.DeserializeAsync<InstrumentPositionBook>(strm, jsonSerializerOptions);

        if (result == null || result.PositionBook == null)
        {
            return new PositionBook();
        }

        return result.PositionBook;
    }
}
