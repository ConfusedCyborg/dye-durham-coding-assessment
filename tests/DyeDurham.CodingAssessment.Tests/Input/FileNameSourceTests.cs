using DyeDurham.CodingAssessment.Input;

namespace DyeDurham.CodingAssessment.Tests.Input;

public sealed class FileNameSourceTests : IDisposable
{
    private readonly TemporaryFolder _folder = new();

    public void Dispose() => _folder.Dispose();

    [Theory]
    [InlineData("Janet Parsons\nVaughn Lewis\n", new[] { "Janet Parsons", "Vaughn Lewis" })]
    [InlineData("Janet Parsons\r\nVaughn Lewis\r\n", new[] { "Janet Parsons", "Vaughn Lewis" })]
    [InlineData("Janet Parsons\n\nVaughn Lewis", new[] { "Janet Parsons", "", "Vaughn Lewis" })]
    [InlineData("", new string[0])]
    public void ReadLines_FileContent_ReturnsEveryLineWithoutLineEndings(string content, string[] expected)
    {
        string path = _folder.File("names.txt");
        File.WriteAllText(path, content);

        IEnumerable<string> lines = new FileNameSource(path).ReadLines();

        Assert.Equal(expected, lines);
    }

    [Fact]
    public void ReadLines_MissingFile_ThrowsFileNotFoundException()
    {
        FileNameSource source = new(_folder.File("missing.txt"));

        Assert.Throws<FileNotFoundException>(() => source.ReadLines());
    }
}
