using System.Text;
using DyeDurham.CodingAssessment;
using DyeDurham.CodingAssessment.IO;
using DyeDurham.CodingAssessment.Parsing;
using DyeDurham.CodingAssessment.Sorting;

// Usage: name-sorter <path to a file with one name per line>
// Prints the sorted names and writes them to sorted-names-list.txt in the current folder.

const string SortedFileName = "sorted-names-list.txt";

// Without this, accented names print as garbage on Windows when the output is piped or redirected.
Console.OutputEncoding = Encoding.UTF8;

if (args.Length != 1)
{
    return Fail(ExitCodes.WrongArguments, "Usage: name-sorter <path to a file with one name per line>");
}

string unsortedFilePath = args[0];

if (!File.Exists(unsortedFilePath))
{
    return Fail(ExitCodes.FileNotFound, $"Could not find the file \"{unsortedFilePath}\".");
}

try
{
    CreateService(unsortedFilePath).Run();
    return ExitCodes.Success;
}
catch (InvalidNameException exception)
{
    return Fail(ExitCodes.InvalidName, $"Could not sort the names. {exception.Message}");
}
catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
{
    // For example a locked input file, or a read-only sorted-names-list.txt.
    return Fail(ExitCodes.FileError, $"Could not read or write a file. {exception.Message}");
}

// Composition root: the only place that picks the concrete classes.
static NameSortingService CreateService(string unsortedFilePath) =>
    new(
        new FileNameSource(unsortedFilePath),
        new NameParser(),
        new NameSorter(),
        new INameOutput[]
        {
            new ConsoleNameOutput(Console.Out),
            new FileNameOutput(SortedFileName),
        });

static int Fail(int exitCode, string message)
{
    Console.Error.WriteLine(message);
    return exitCode;
}
