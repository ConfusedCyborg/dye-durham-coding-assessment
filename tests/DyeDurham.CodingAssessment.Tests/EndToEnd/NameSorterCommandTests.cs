using System.Diagnostics;
using System.Text;

namespace DyeDurham.CodingAssessment.Tests.EndToEnd;

/// <summary>
/// Runs the real name-sorter program, the way the brief does, inside a temporary working folder.
/// </summary>
public sealed class NameSorterCommandTests : IDisposable
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(30);

    private static readonly string[] ExpectedSortedExample =
    {
        "Marin Alvarez",
        "Adonis Julius Archer",
        "Beau Tristan Bentley",
        "Hunter Uriah Mathew Clarke",
        "Leo Gardner",
        "Vaughn Lewis",
        "London Lindsey",
        "Mikayla Lopez",
        "Janet Parsons",
        "Frankie Conner Ritter",
        "Shelby Nathan Yoder",
    };

    private readonly TemporaryFolder _workingFolder = new();

    public void Dispose() => _workingFolder.Dispose();

    [Fact]
    public void Prints_the_example_from_the_brief_sorted_to_the_screen()
    {
        CopyExampleIntoWorkingFolder();

        var result = RunNameSorter("./unsorted-names-list.txt");

        Assert.Equal(0, result.ExitCode);
        Assert.Equal(ExpectedSortedExample, Lines(result.Output));
    }

    [Fact]
    public void Writes_the_example_from_the_brief_sorted_to_a_file_in_the_working_folder()
    {
        CopyExampleIntoWorkingFolder();

        RunNameSorter("./unsorted-names-list.txt");

        Assert.Equal(ExpectedSortedExample, File.ReadAllLines(_workingFolder.File("sorted-names-list.txt")));
    }

    [Fact]
    public void Prints_accented_names_correctly()
    {
        File.WriteAllLines(_workingFolder.File("names.txt"), new[] { "Émile Émond" });

        var result = RunNameSorter("./names.txt");

        Assert.Equal(new[] { "Émile Émond" }, Lines(result.Output));
    }

    [Fact]
    public void Writes_an_empty_file_when_the_input_is_empty()
    {
        File.WriteAllText(_workingFolder.File("names.txt"), "");

        var result = RunNameSorter("./names.txt");

        Assert.Equal(0, result.ExitCode);
        Assert.Empty(File.ReadAllLines(_workingFolder.File("sorted-names-list.txt")));
    }

    [Fact]
    public void Shows_how_to_use_it_when_given_no_file()
    {
        var result = RunNameSorter();

        Assert.Equal(1, result.ExitCode);
        Assert.Contains("Usage: name-sorter", result.Error);
    }

    [Fact]
    public void Shows_how_to_use_it_when_given_two_files()
    {
        var result = RunNameSorter("first.txt", "second.txt");

        Assert.Equal(1, result.ExitCode);
        Assert.Contains("Usage: name-sorter", result.Error);
    }

    [Fact]
    public void Explains_when_the_file_does_not_exist()
    {
        var result = RunNameSorter("./missing.txt");

        Assert.Equal(2, result.ExitCode);
        Assert.Contains("missing.txt", result.Error);
    }

    [Fact]
    public void Explains_which_line_holds_an_invalid_name_and_writes_no_file()
    {
        File.WriteAllLines(_workingFolder.File("names.txt"), new[] { "Janet Parsons", "Madonna" });

        var result = RunNameSorter("./names.txt");

        Assert.Equal(3, result.ExitCode);
        Assert.Contains("Line 2", result.Error);
        Assert.False(File.Exists(_workingFolder.File("sorted-names-list.txt")));
    }

    [Fact]
    public void Explains_when_the_sorted_file_cannot_be_written()
    {
        CopyExampleIntoWorkingFolder();
        var sortedFile = _workingFolder.File("sorted-names-list.txt");
        File.WriteAllText(sortedFile, "");
        File.SetAttributes(sortedFile, FileAttributes.ReadOnly);

        var result = RunNameSorter("./unsorted-names-list.txt");

        Assert.Equal(4, result.ExitCode);
        Assert.Contains("sorted-names-list.txt", result.Error);
    }

    private void CopyExampleIntoWorkingFolder()
    {
        var example = Path.Combine(AppContext.BaseDirectory, "TestData", "unsorted-names-list.txt");
        File.Copy(example, _workingFolder.File("unsorted-names-list.txt"));
    }

    private CommandResult RunNameSorter(params string[] arguments)
    {
        var startInfo = new ProcessStartInfo("dotnet")
        {
            WorkingDirectory = _workingFolder.Path,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8,
        };
        startInfo.ArgumentList.Add(Path.Combine(AppContext.BaseDirectory, "name-sorter.dll"));
        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        using var process = Process.Start(startInfo)!;
        var output = process.StandardOutput.ReadToEndAsync();
        var error = process.StandardError.ReadToEndAsync();

        if (!process.WaitForExit(Timeout))
        {
            process.Kill(entireProcessTree: true);
            Assert.Fail($"name-sorter did not finish within {Timeout.TotalSeconds} seconds.");
        }

        return new CommandResult(process.ExitCode, output.Result, error.Result);
    }

    private static string[] Lines(string text) =>
        text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

    private sealed record CommandResult(int ExitCode, string Output, string Error);
}
