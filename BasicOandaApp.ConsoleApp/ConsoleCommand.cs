using BasicOandaApp.ConsoleApp.Helpers;
using NLog;
using System.Text.Json;

namespace BasicOandaApp.ConsoleApp;

//Sample usage documentation:
//imageconv:
//Converts an image file from one format to another.
//Usage:
//  imageconv[options]
//Options:
//  --input          The path to the image file that is to be converted.
//  --output         The target name of the output after conversion.
//  --x-crop-size    The X dimension size to crop the picture.
//                   The default is 0 indicating no cropping is required.
//  --y-crop-size    The Y dimension size to crop the picture.
//                   The default is 0 indicating no cropping is required.
//  --version        Display version information

// Rules for parsing arguments
// Example call:
// dotnet run --project .\BasicOandaApp.ConsoleApp\ -- someObj -i someInput --input someInput2 -o "some output" obj2 /ad /zxc "zxcVelu" --asd-zxc someValue
// Rules for parsing arguments
// arg[0] will always equal to full path of executable (eg. "C:\src\github.com\ongzhixian\BasicOandaApp\BasicOandaApp.ConsoleApp\bin\Debug\net6.0\BasicOandaApp.ConsoleApp.dll")
// Key-value pairs
//  -<char> <value>     => short form; value with spaces should be in quotes (single or double; not mixed)
// --<word> <value>     => long form
// <target>             => should only have one target value; should be in quotes (single or double; not mixed)
// Options:
// --dry-run

/***********************************************************************
Usage documentation:
consoleApp:
Use for starting algorithmic trading or dump information.
Usage:
  consoleApp[options]
Options:
  --dump           Dump specific information where information could be one of the following:
                     a) tradables instruments 
                     b) historical data (candles)
  --output         Output folder (relative to execution); defaults to "./dump"
  --instrument     Instrument to dump information for
  --granularity    Granularity to dump information for
  --version        Display version information
  TO IMPLEMENT
  --outfile        Output filename (relative to execution); defaults to "./dump/outfile.txt"
  --help           Display help

Example calls:
1)  No arguments; starts algorithmic trading.
dotnet run --project .\BasicOandaApp.ConsoleApp\
2)  Prints version and terminate.
dotnet run --project .\BasicOandaApp.ConsoleApp\ -- --version
3)  Prints version, dumps instruments to ./dump folder and terminate.
dotnet run --project .\BasicOandaApp.ConsoleApp\ -- --version --dump instruments --output ./dump
4)  Prints version, dumps candles to ./dump folder and terminate.
dotnet run --project .\BasicOandaApp.ConsoleApp\ -- --version --dump candles --output ./dump --instrument XAU_USD --granularity H1


S5 	    5 second candlesticks, minute alignment
S10 	10 second candlesticks, minute alignment
S15 	15 second candlesticks, minute alignment
S30 	30 second candlesticks, minute alignment
M1 	    1 minute candlesticks, minute alignment
M2 	    2 minute candlesticks, hour alignment
M4 	    4 minute candlesticks, hour alignment
M5 	    5 minute candlesticks, hour alignment
M10 	10 minute candlesticks, hour alignment
M15 	15 minute candlesticks, hour alignment
M30 	30 minute candlesticks, hour alignment
H1 	    1 hour candlesticks, hour alignment
H2 	    2 hour candlesticks, day alignment
H3 	    3 hour candlesticks, day alignment
H4 	    4 hour candlesticks, day alignment
H6 	    6 hour candlesticks, day alignment
H8 	    8 hour candlesticks, day alignment
H12 	12 hour candlesticks, day alignment
D 	    1 day candlesticks, day alignment
W 	    1 week candlesticks, aligned to start of week
M 	    1 month candlesticks, aligned to first day of the month
*/

internal abstract class ConsoleCommand
{
    public enum COMMAND_PRIORITY
    { 
        PRINT = 1,
        OPTION = 2,
        ACTION = 3,
    }

    public COMMAND_PRIORITY Priority { get; private set; }

    public string Name { get; private set; }

    public string? Argument { get; private set; }

    protected ConsoleCommand(COMMAND_PRIORITY priority, string name)
    {
        this.Priority = priority;
        this.Name = name;
    }

    protected ConsoleCommand(COMMAND_PRIORITY priority, string name, string argument) : this(priority, name)
    {
        this.Argument = argument;
    }

    public abstract void Execute();

    public override string ToString()
    {
        return $"{Priority} - {Name} {Argument}";
    }
}

internal class VersionConsoleCommand : ConsoleCommand
{
    private static Logger log = LogManager.GetCurrentClassLogger();

    public const string NAME = "VERSION";
    
    public VersionConsoleCommand() : base(COMMAND_PRIORITY.PRINT, NAME) { }

    public override void Execute()
    {
        log.Info("Version {version}", AppState.Version);
    }
}

internal class HelpConsoleCommand : ConsoleCommand
{
    private static Logger log = LogManager.GetCurrentClassLogger();

    public const string NAME = "HELP";
    
    public HelpConsoleCommand() : base(COMMAND_PRIORITY.PRINT, NAME) { }

    public override void Execute()
    {
        // TODO: Pending implementation
    }
}

internal class DumpConsoleCommand : ConsoleCommand
{
    private static Logger log = LogManager.GetCurrentClassLogger();

    public const string NAME = "DUMP";

    public DumpConsoleCommand(string argument) : base(COMMAND_PRIORITY.ACTION, NAME, argument) { }

    public override void Execute()
    {
        JsonSerializerOptions options = new JsonSerializerOptions(JsonSerializerDefaults.General)
        {
            WriteIndented = true,
        };

        var outputDirectoryFullPath = FileSystemHelper.GetEnsuredFullPath(CommandState.OutputDirectoryPath);

        var dataType = this.Argument.ToUpperInvariant();

        switch (dataType)
        {
            case "INSTRUMENTS":
                Task.Run(async () =>
                {
                    using JsonDocument? jsonDocument = JsonDocument.Parse(await AppState.OandaRestApi.DumpTradableInstrumentListAsync());
                    using StreamWriter sw = new(Path.Combine(outputDirectoryFullPath, "instruments.json"));
                    sw.AutoFlush = true;
                    sw.Write(JsonSerializer.Serialize(jsonDocument, options));
                }).Wait();
                break;
            case "CANDLES":
                Task.Run(async () =>
                {
                    using JsonDocument? jsonDocument = JsonDocument.Parse(
                        await AppState.OandaRestApi.DumpInstrumentCandlesForAccountAsync(CommandState.Instrument, CommandState.Granularity));
                    using StreamWriter sw = new(Path.Combine(outputDirectoryFullPath, $"{CommandState.Instrument}-{CommandState.Granularity}-candles.json"));
                    sw.AutoFlush = true;
                    sw.Write(JsonSerializer.Serialize(jsonDocument, options));
                }).Wait();
                break;
            default:
                log.Warn("Unknown data type {dataType}", dataType);
                break;
        }
    }
}

internal class OutputConsoleCommand : ConsoleCommand
{
    private static Logger log = LogManager.GetCurrentClassLogger();

    public const string NAME = "OUTPUT";

    public OutputConsoleCommand(string argument) : base(COMMAND_PRIORITY.OPTION, NAME, argument) { }

    public override void Execute()
    {
        if (Argument != null) 
        {
            CommandState.OutputDirectoryPath = Argument;
        }

        log.Info("{information} is {OutputDirectoryPath}", NAME, CommandState.OutputDirectoryPath);
    }
}

internal class InstrumentConsoleCommand : ConsoleCommand
{
    private static Logger log = LogManager.GetCurrentClassLogger();

    public const string NAME = "INSTRUMENT";

    public InstrumentConsoleCommand(string argument) : base(COMMAND_PRIORITY.OPTION, NAME, argument) { }

    public override void Execute()
    {
        if (Argument != null)
        {
            CommandState.Instrument = Argument;
        }

        log.Info("{information} is {Instrument}", NAME, CommandState.Instrument);
    }
}

internal class GranularityConsoleCommand : ConsoleCommand
{
    private static Logger log = LogManager.GetCurrentClassLogger();

    public const string NAME = "GRANULARITY";

    public GranularityConsoleCommand(string argument) : base(COMMAND_PRIORITY.OPTION, NAME, argument) { }

    public override void Execute()
    {
        if (Argument != null)
        {
            CommandState.Granularity = Argument;
        }

        log.Info("{information} is {Granularity}", NAME, CommandState.Granularity);
    }
}

internal static class ConsoleCommandParser
{
    private static Logger log = LogManager.GetCurrentClassLogger();

    public static IOrderedEnumerable<ConsoleCommand> GetConsoleCommands()
    {
        List<ConsoleCommand> commands = new List<ConsoleCommand>();

        string[] arguments = Environment.GetCommandLineArgs();
        
        for (int argIndex = 1; argIndex < arguments.Length; argIndex++)
        {
            if (arguments[argIndex].StartsWith("--"))
            {
                string command = arguments[argIndex].Remove(0, 2).ToUpperInvariant();

                switch (command)
                {
                    case VersionConsoleCommand.NAME:
                        commands.Add(new VersionConsoleCommand());
                        break;
                    case DumpConsoleCommand.NAME:
                        commands.Add(new DumpConsoleCommand(GetCommandArgument(arguments, argIndex)));
                        break;
                    case OutputConsoleCommand.NAME:
                        commands.Add(new OutputConsoleCommand(GetCommandArgument(arguments, argIndex)));
                        break;
                    case InstrumentConsoleCommand.NAME:
                        commands.Add(new InstrumentConsoleCommand(GetCommandArgument(arguments, argIndex)));
                        break;
                    default:
                        log.Warn("Unknown command {command}", command);
                        break;
                }
            }
        }

        return commands.OrderBy(a => a.Priority);
    }

    private static string GetCommandArgument(string[] arguments, int argIndex)
    {
        if (arguments.Length < argIndex + 1)
        {
            throw new ArgumentException("Invalid number of arguments");
        }

        return arguments[argIndex + 1];
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           