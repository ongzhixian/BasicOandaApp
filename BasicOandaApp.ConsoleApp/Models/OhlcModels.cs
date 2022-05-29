using System.Text.Json.Serialization;

namespace BasicOandaApp.ConsoleApp.Models;

internal class Ohlc
{
    [JsonPropertyName("t")]
    public DateTime Time { get; set; }

    [JsonPropertyName("o")]
    public decimal O { get; set; }

    [JsonPropertyName("h")]
    public decimal H { get; set; }

    [JsonPropertyName("l")]
    public decimal L { get; set; }

    [JsonPropertyName("c")]
    public decimal C { get; set; }

    [JsonPropertyName("volume")]
    public int Volume { get; set; }

    [JsonPropertyName("complete")]
    public bool Complete { get; set; }
}


internal class OhlcDataFrame
{
    //const string TIME_COLUMN_NAME = "t"; const int TIME_COLUMN_INDEX = 0;
    //const string OPEN_COLUMN_NAME = "o"; const int OPEN_COLUMN_INDEX = 1;
    //const string HIGH_COLUMN_NAME = "h"; const int HIGH_COLUMN_INDEX = 2;
    //const string LOW_COLUMN_NAME = "l"; const int LOW_COLUMN_INDEX = 3;
    //const string CLOSE_COLUMN_NAME = "c"; const int CLOSE_COLUMN_INDEX = 4;
    //const string VOLUME_COLUMN_NAME = "v"; const int VOLUME_COLUMN_INDEX = 5;
    //const string COMPLETE_COLUMN_NAME = "e"; const int COMPLETE_COLUMN_INDEX = 6;

    public const string TIME_COLUMN_NAME = "t";
    public const string OPEN_COLUMN_NAME = "o";
    public const string HIGH_COLUMN_NAME = "h";
    public const string LOW_COLUMN_NAME = "l";
    public const string CLOSE_COLUMN_NAME = "c";
    public const string VOLUME_COLUMN_NAME = "v";
    public const string COMPLETE_COLUMN_NAME = "e";

    public const int TIME_COLUMN_INDEX = 0;
    public const int OPEN_COLUMN_INDEX = 1;
    public const int HIGH_COLUMN_INDEX = 2;
    public const int LOW_COLUMN_INDEX = 3;
    public const int CLOSE_COLUMN_INDEX = 4;
    public const int VOLUME_COLUMN_INDEX = 5;
    public const int COMPLETE_COLUMN_INDEX = 6;
}