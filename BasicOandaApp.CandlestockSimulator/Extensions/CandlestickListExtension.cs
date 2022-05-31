using BasicOandaApp.CandlestockSimulator.Models;

namespace BasicOandaApp.CandlestockSimulator.Extensions;

internal static class CandlestickListExtension
{
    public static IEnumerable<Ohlc> ToOhlcList(this IList<OandaCandlestick> candlestickList)
    {
        if (candlestickList.Any(r => r.Mid == null))
        {
            throw new ArgumentException($"{nameof(candlestickList)} contains null data");
        }

        return candlestickList.Select(r => new Ohlc
        {
            Time = r.Time,
            O = r.Mid?.O ?? 0M, // should have been just `r.Mid.O`; writing it this way to make analyzer happy.
            H = r.Mid?.H ?? 0M,
            L = r.Mid?.L ?? 0M,
            C = r.Mid?.C ?? 0M,
            Complete = r.Complete,
            Volume = r.Volume
        });
    }

    public static IEnumerable<LabelledOhlc> ToLabelledOhlcList(this IList<OandaCandlestick> candlestickList)
    {
        if (candlestickList.Any(r => r.Mid == null))
        {
            throw new ArgumentException($"{nameof(candlestickList)} contains null data");
        }

        return candlestickList.Select(r => new LabelledOhlc
        {
            Time = r.Time,
            O = r.Mid?.O ?? 0M, // should have been just `r.Mid.O`; writing it this way to make analyzer happy.
            H = r.Mid?.H ?? 0M,
            L = r.Mid?.L ?? 0M,
            C = r.Mid?.C ?? 0M,
            Complete = r.Complete,
            Volume = r.Volume
        });
    }
}
