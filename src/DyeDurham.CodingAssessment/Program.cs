using System.Text;
using DyeDurham.CodingAssessment;
using DyeDurham.CodingAssessment.IO;
using DyeDurham.CodingAssessment.Parsing;
using DyeDurham.CodingAssessment.Sorting;

// Entry point and composition root: the only place that picks the concrete classes.

const string SortedFileName = "sorted-names-list.txt";

// Without this, accented names print as garbage on Windows when the output is piped or redirected.
Console.OutputEncoding = Encoding.UTF8;

if (args.Length != 1)
{
    Console.Error.WriteLine("Usage: name-sorter <path to a file with one name per line>");
    return ExitCodes.WrongArguments;
}

var unsortedFilePath = args[0];

if (!File.Exists(unsortedFilePath))
{
    Console.Error.WriteLine($"Could not find the file \"{unsortedFilePath}\".");
    return ExitCodes.FileNotFound;
}

var service = new NameSortingService(
    new FileNameSource(unsortedFilePath),
    new NameParser(),
    new NameSorter(new PersonNameComparer()),
    new INameOutput[]
    {
        new ConsoleNameOutput(Console.Out),
        new FileNameOutput(SortedFileName),
    });

try
{
    service.Run();
    return ExitCodes.Success;
}
catch (InvalidNameException exception)
{
    Console.Error.WriteLine($"Could not sort the names. {exception.Message}");
    return ExitCodes.InvalidName;
}
catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
{
    // For example a locked input file, or a read-only sorted-names-list.txt.
    Console.Error.WriteLine($"Could not read or write a file. {exception.Message}");
    return ExitCodes.FileError;
}
