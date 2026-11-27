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
    public void Run_BriefExample_PrintsAndWritesSortedNamesAndExitsWith0()
    {
        CopyExampleIntoWorkingFolder();

        CommandResult result = RunNameSorter("./unsorted-names-list.txt");

        Assert.Equal(0, result.ExitCode);
        Assert.Equal(ExpectedSortedExample, Lines(result.Output));
        Assert.Equal(ExpectedSortedExample, File.ReadAllLines(_workingFolder.File("sorted-names-list.txt")));
    }

    [Theory]
    [InlineData("Janet Parsons\nMarin Alvarez\n", new[] { "Marin Alvarez", "Janet Parsons" })]
    [InlineData("Émile Émond\nAdam Fisher\n", new[] { "Émile Émond", "Adam Fisher" })]
    [InlineData("", new string[0])]
    public void Run_ValidFile_PrintsAndWritesSortedNamesAndExitsWith0(string content, string[] expected)
    {
        File.WriteAllText(_workingFolder.File("names.txt"), content);

        CommandResult result = RunNameSorter("./names.txt");

        Assert.Equal(0, result.ExitCode);
        Assert.Equal(expected, Lines(result.Output));
        Assert.Equal(expected, File.ReadAllLines(_workingFolder.File("sorted-names-list.txt")));
    }

    public static TheoryData<string[]> WrongArgumentCounts => new()
    {
        Array.Empty<string>(),
        new[] { "first.txt", "second.txt" },
    };

    [Theory]
    [MemberData(nameof(WrongArgumentCounts))]
    public void Run_NotExactlyOneArgument_PrintsUsageAndExitsWith1(string[] arguments)
    {
        CommandResult result = RunNameSorter(arguments);

        Assert.Equal(1, result.ExitCode);
        Assert.Contains("Usage: name-sorter", result.Error);
    }

    [Theory]
    [InlineData("./missing.txt")]
    [InlineData("./missing-folder/names.txt")]
    public void Run_MissingFile_NamesFileAndExitsWith2(string path)
    {
        CommandResult result = RunNameSorter(path);

        Assert.Equal(2, result.ExitCode);
        Assert.Contains(path, result.Error);
    }

    [Theory]
    [InlineData("Madonna")]
    [InlineData("One Two Three Four Last")]
    public void Run_InvalidName_ReportsLineAndExitsWith3WithoutWritingFile(string invalidLine)
    {
        File.WriteAllLines(_workingFolder.File("names.txt"), new[] { "Janet Parsons", invalidLine });

        CommandResult result = RunNameSorter("./names.txt");

        Assert.Equal(3, result.ExitCode);
        Assert.Contains("Line 2", result.Error);
        Assert.False(File.Exists(_workingFolder.File("sorted-names-list.txt")));
    }

    [Fact]
    public void Run_SeveralInvalidNames_ReportsEveryLineAndExitsWith3()
    {
        File.WriteAllLines(_workingFolder.File("names.txt"), new[] { "Madonna", "Janet Parsons", "One Two Three Four Last" });

        CommandResult result = RunNameSorter("./names.txt");

        Assert.Equal(3, result.ExitCode);
        Assert.Contains("Line 1 (\"Madonna\")", result.Error);
        Assert.Contains("Line 3 (\"One Two Three Four Last\")", result.Error);
    }

    [Fact]
    public void Run_ReadOnlyOutputFile_ExitsWith4WithoutPrintingNames()
    {
        CopyExampleIntoWorkingFolder();
        string sortedFile = _workingFolder.File("sorted-names-list.txt");
        File.WriteAllText(sortedFile, "");
        File.SetAttributes(sortedFile, FileAttributes.ReadOnly);

        CommandResult result = RunNameSorter("./unsorted-names-list.txt");

        Assert.Equal(4, result.ExitCode);
        Assert.Contains("sorted-names-list.txt", result.Error);
        Assert.Empty(result.Output);
    }

    private void CopyExampleIntoWorkingFolder()
    {
        string example = Path.Combine(AppContext.BaseDirectory, "TestData", "unsorted-names-list.txt");
        File.Copy(example, _workingFolder.File("unsorted-names-list.txt"));
    }

    private CommandResult RunNameSorter(params string[] arguments)
    {
        ProcessStartInfo startInfo = new("dotnet")
        {
            WorkingDirectory = _workingFolder.Path,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8,
        };
        startInfo.ArgumentList.Add(Path.Combine(AppContext.BaseDirectory, "name-sorter.dll"));
        foreach (string argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        using Process process = Process.Start(startInfo)!;
        Task<string> output = process.StandardOutput.ReadToEndAsync();
        Task<string> error = process.StandardError.ReadToEndAsync();

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
