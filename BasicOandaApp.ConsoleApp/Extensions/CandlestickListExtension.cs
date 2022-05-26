using BasicOandaApp.ConsoleApp.Models;
using Oanda.RestApi.Models;

namespace BasicOandaApp.ConsoleApp.Extensions;

internal static class ListCandlestickExtension
{
    // IList<Candlestick>
    public static IEnumerable<Ohlc> ToOhlcList(this IList<Candlestick> candlestickList, CandlestickType candlestickType) => candlestickType switch
    {
        CandlestickType.Bid => candlestickList.Select(r => new Ohlc
        {
            O = r.Bid.O,
            H = r.Bid.H,
            L = r.Bid.L,
            C = r.Bid.C,
            Complete = r.Complete,
            Volume = r.Volume
        }),
        CandlestickType.Ask => candlestickList.Select(r => new Ohlc
        {
            O = r.Ask.O,
            H = r.Ask.H,
            L = r.Ask.L,
            C = r.Ask.C,
            Complete = r.Complete,
            Volume = r.Volume
        }),
        CandlestickType.Mid => candlestickList.Select(r => new Ohlc
        {
            O = r.Mid.O,
            H = r.Mid.H,
            L = r.Mid.L,
            C = r.Mid.C,
            Complete = r.Complete,
            Volume = r.Volume
        })
    };

    public static IEnumerable<Ohlc> ToOhlcList(this IList<Candlestick> candlestickList) =>
        candlestickList.Select(r => new Ohlc
        {
            O = r.Mid.O,
            H = r.Mid.H,
            L = r.Mid.L,
            C = r.Mid.C,
            Complete = r.Complete,
            Volume = r.Volume
        })
}
