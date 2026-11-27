using DyeDurham.CodingAssessment.Output;

namespace DyeDurham.CodingAssessment.Tests.Output;

public sealed class FileNameOutputTests : IDisposable
{
    private readonly TemporaryFolder _folder = new();

    public void Dispose() => _folder.Dispose();

    public static TheoryData<string[]> NameLists => new()
    {
        Array.Empty<string>(),
        new[] { "Marin Alvarez" },
        new[] { "Marin Alvarez", "Adonis Julius Archer" },
    };

    [Theory]
    [MemberData(nameof(NameLists))]
    public void Write_Names_WritesOneFullNamePerLine(string[] fullNames)
    {
        string path = _folder.File("sorted.txt");

        new FileNameOutput(path).Write(TestNames.From(fullNames));

        Assert.Equal(fullNames, File.ReadAllLines(path));
    }

    [Fact]
    public void Write_FileAlreadyExists_ReplacesItsContent()
    {
        string path = _folder.File("sorted.txt");
        File.WriteAllLines(path, new[] { "Old Name", "Another Old Name", "Third Old Name" });

        new FileNameOutput(path).Write(TestNames.From("Marin Alvarez"));

        Assert.Equal(new[] { "Marin Alvarez" }, File.ReadAllLines(path));
    }

    [Fact]
    public void Write_ReadOnlyFile_ThrowsUnauthorizedAccessException()
    {
        string path = _folder.File("sorted.txt");
        File.WriteAllText(path, "");
        File.SetAttributes(path, FileAttributes.ReadOnly);

        FileNameOutput output = new(path);

        Assert.Throws<UnauthorizedAccessException>(() => output.Write(TestNames.From("Marin Alvarez")));
    }
}
