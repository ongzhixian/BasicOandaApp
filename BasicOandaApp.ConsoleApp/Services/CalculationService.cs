using Microsoft.Data.Analysis;

namespace BasicOandaApp.ConsoleApp.Services;

internal static class CalculationService
{
    // Rolling/Sliding window SMA   (or Windowed-SMA WSMA)
    // vs Running SMA               (RSMA)
    // SMA vs EMA

    public static void Sma(int window, DataFrameColumn src, DataFrameColumn dst)
    {
        long rowCount = src.Length;

        if (dst.Length != src.Length)
        {
            throw new InvalidOperationException("src and dst have different length.");
        }

        if (window > rowCount)
        {
            throw new InvalidOperationException("window > src.length");
        }

        int idx;
        decimal sum = 0;

        // sum of 1st first window 
        for (idx = 0; idx < window; idx++) // 0, 1, 2, 3
        {
            sum = sum + (decimal)src[idx];
        }

        // MA for first window
        dst[window - 1] = sum / window;

        // MA for subsequent rolling window
        for (idx = window; idx < rowCount; idx++) // 4, 5, 6
        {
            sum = sum + (decimal)src[idx] - (decimal)src[idx - window];

            dst[idx] = sum / window;
        }
    }
}
