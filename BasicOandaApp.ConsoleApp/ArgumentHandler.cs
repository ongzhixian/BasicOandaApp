namespace BasicOandaApp.ConsoleApp;


// Parsing arguments
// dotnet run --project .\BasicOandaApp.ConsoleApp\ -- someObj -i someInput --input someInput2 -o "some output" obj2 /ad /zxc "zxcVelu" --asd-zxc someValue
// Rules for parsing arguments
// arg[0] will always equal to full path of executable (eg. "C:\src\github.com\ongzhixian\BasicOandaApp\BasicOandaApp.ConsoleApp\bin\Debug\net6.0\BasicOandaApp.ConsoleApp.dll")
// Key-value pairs
//  -<char> <value>     => short form; value with spaces should be in quotes (single or double; not mixed)
// --<word> <value>     => long form
// <target>             => should only have one target value; should be in quotes (single or double; not mixed)
// Options:
// --dry-run

//imageconv:
//Converts an image file from one format to another.
//Usage:
//  imageconv[options]
//Options:
//--input          The path to the image file that is to be converted.
//  --output         The target name of the output after conversion.
//  --x-crop-size    The X dimension size to crop the picture.
//                   The default is 0 indicating no cropping is required.
//  --y-crop-size    The Y dimension size to crop the picture.
//                   The default is 0 indicating no cropping is required.
//  --version        Display version information


//string[] arguments = Environment.GetCommandLineArgs();
//Console.WriteLine("GetCommandLineArgs: {0}", string.Join(", ", arguments));

//int i = 0;
//Console.WriteLine(arguments.Length);
//foreach (var item in arguments)
//{
//    Console.WriteLine("{0}={1}", i++, item);
//}


internal class ArgumentHandler
{
}
