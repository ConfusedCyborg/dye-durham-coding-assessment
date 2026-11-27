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
}
