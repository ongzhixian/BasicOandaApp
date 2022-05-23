using BasicOandaApp.ConsoleApp.Helpers;
using NLog;

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
                     b) historical data
  --output         Output folder (relative to execution); defaults to "./dump"
  --outfile        Output filename (relative to execution); defaults to "./dump/outfile.txt"
  --version        Display version information

Example calls:
1)  No arguments; starts algorithmic trading.
dotnet run --project .\BasicOandaApp.ConsoleApp\
2)  Prints version and terminate.
dotnet run --project .\BasicOandaApp.ConsoleApp\ -- --version
3)  Prints version, dumps instruments to ./dump folder and terminate.
dotnet run --project .\BasicOandaApp.ConsoleApp\ -- --version --dump instruments --output ./dump

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

internal class DumpConsoleCommand : ConsoleCommand
{
    public const string NAME = "DUMP";

    public DumpConsoleCommand(string argument) : base(COMMAND_PRIORITY.ACTION, NAME, argument) { }

    public override void Execute()
    {
        var outputDirectoryFullPath = FileSystemHelper.GetEnsuredFullPath(CommandState.OutputDirectoryPath);

        string outputFileFullPath = Path.Combine(outputDirectoryFullPath, "instruments.txt");

        using StreamWriter sw = new StreamWriter(outputFileFullPath);

        sw.AutoFlush = true;

        sw.WriteLine("Test data");
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

        log.Info("{information} is {OutputDirectoryPath}", "Output directory", CommandState.OutputDirectoryPath);
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
