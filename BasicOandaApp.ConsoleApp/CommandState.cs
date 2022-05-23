namespace BasicOandaApp.ConsoleApp;

internal static class CommandState
{
    public static string OutputDirectoryPath { get; set; } = "./dump";

    public static string Instrument { get; set; } = "XAU_USD";

    public static string Granularity { get; set; } = "H1";
}
