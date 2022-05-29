using BasicOandaApp.ConsoleApp.Models;
using Microsoft.Data.Analysis;
using Oanda.RestApi.Models;

namespace BasicOandaApp.ConsoleApp.Extensions;

internal static class ListCandlestickExtension
{
    // IList<Candlestick>
    public static IEnumerable<Ohlc> ToOhlcList(this IList<Candlestick> candlestickList, CandlestickType candlestickType) => candlestickType switch
    {
        CandlestickType.Bid => candlestickList.Select(r => new Ohlc
        {
            Time = r.Time,
            O = r.Bid.O,
            H = r.Bid.H,
            L = r.Bid.L,
            C = r.Bid.C,
            Complete = r.Complete,
            Volume = r.Volume
        }),
        CandlestickType.Ask => candlestickList.Select(r => new Ohlc
        {
            Time = r.Time,
            O = r.Ask.O,
            H = r.Ask.H,
            L = r.Ask.L,
            C = r.Ask.C,
            Complete = r.Complete,
            Volume = r.Volume
        }),
        CandlestickType.Mid => candlestickList.Select(r => new Ohlc
        {
            Time = r.Time,
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
            Time = r.Time,
            O = r.Mid.O,
            H = r.Mid.H,
            L = r.Mid.L,
            C = r.Mid.C,
            Complete = r.Complete,
            Volume = r.Volume
        });

    public static DataFrame ToDataFrame(this IList<Candlestick> candlestickList) =>
        new (
            new PrimitiveDataFrameColumn<DateTime>(OhlcDataFrame.TIME_COLUMN_NAME, candlestickList.Select(r => r.Time)),
            new PrimitiveDataFrameColumn<decimal>(OhlcDataFrame.OPEN_COLUMN_NAME, candlestickList.Select(r => r.Mid.O)),
            new PrimitiveDataFrameColumn<decimal>(OhlcDataFrame.HIGH_COLUMN_NAME, candlestickList.Select(r => r.Mid.H)),
            new PrimitiveDataFrameColumn<decimal>(OhlcDataFrame.LOW_COLUMN_NAME, candlestickList.Select(r => r.Mid.L)),
            new PrimitiveDataFrameColumn<decimal>(OhlcDataFrame.CLOSE_COLUMN_NAME, candlestickList.Select(r => r.Mid.C)),
            new PrimitiveDataFrameColumn<int>(OhlcDataFrame.VOLUME_COLUMN_NAME, candlestickList.Select(r => r.Volume)),
            new PrimitiveDataFrameColumn<bool>(OhlcDataFrame.COMPLETE_COLUMN_NAME, candlestickList.Select(r => r.Complete))
        );
}