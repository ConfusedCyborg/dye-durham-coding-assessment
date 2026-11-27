using DyeDurham.CodingAssessment.IO;

namespace DyeDurham.CodingAssessment.Tests.IO;

public sealed class FileNameSourceTests : IDisposable
{
    private readonly TemporaryFolder _folder = new();

    public void Dispose() => _folder.Dispose();

    [Fact]
    public void Reads_every_line_of_the_file()
    {
        var path = _folder.File("names.txt");
        File.WriteAllLines(path, new[] { "Janet Parsons", "", "Vaughn Lewis" });

        var lines = new FileNameSource(path).ReadLines();

        Assert.Equal(new[] { "Janet Parsons", "", "Vaughn Lewis" }, lines);
    }

    [Fact]
    public void Reads_files_saved_with_windows_line_endings_on_any_system()
    {
        var path = _folder.File("names.txt");
        File.WriteAllText(path, "Janet Parsons\r\nVaughn Lewis\r\n");

        var lines = new FileNameSource(path).ReadLines();

        Assert.Equal(new[] { "Janet Parsons", "Vaughn Lewis" }, lines);
    }
}
