namespace BasicOandaApp.CandlestockSimulator;

/***********************************************************************
Usage documentation:
consoleApp:
Use for playing back candlestick data.
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


internal static class ConsoleCommandParser
{
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
                    default:
                        Console.WriteLine("Unknown command {command}", command);
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
