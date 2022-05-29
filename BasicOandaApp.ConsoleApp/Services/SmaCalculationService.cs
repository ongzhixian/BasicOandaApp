namespace BasicOandaApp.ConsoleApp.Services;

/// <summary>
/// Copied from https://andrewlock.net/creating-a-simple-moving-average-calculator-in-csharp-1-a-simple-moving-average-calculator/
/// While this implementation "works", it has the short coming of return values for the first window (when you may need it be null)
/// But the usage of ring buffer is nice.
/// Not really using this; but keeping it as a reference of using ring buffer
/// </summary>
internal class SmaCalculationService
{
    private readonly int windowSize;    // rolling window size
    private readonly decimal[] values;  // ring buffer

    private int index = 0;
    private decimal sum = 0;

    public SmaCalculationService(int windowSize)
    {
        if (windowSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(windowSize), "Must be greater than 0");

        this.windowSize = windowSize;
        values = new decimal[windowSize];
    }

    public decimal Update(decimal input)
    {
        // calculate the new sum
        sum = sum - values[index] + input;

        // overwrite the old value with the new one
        values[index] = input;

        // increment the index (wrapping back to 0)
        index = (index + 1) % windowSize;

        // calculate the average
        return ((decimal)sum) / windowSize;
    }
}
