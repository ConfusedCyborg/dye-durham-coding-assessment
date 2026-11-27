using DyeDurham.CodingAssessment.Domain;
using DyeDurham.CodingAssessment.IO;

namespace DyeDurham.CodingAssessment.Tests.IO;

public sealed class FileNameOutputTests : IDisposable
{
    private readonly TemporaryFolder _folder = new();

    public void Dispose() => _folder.Dispose();

    [Fact]
    public void Write_TwoNames_WritesOneFullNamePerLine()
    {
        var path = _folder.File("sorted.txt");

        new FileNameOutput(path).Write(new[] { Name("Marin", "Alvarez"), Name("Adonis Julius", "Archer") });

        Assert.Equal(new[] { "Marin Alvarez", "Adonis Julius Archer" }, File.ReadAllLines(path));
    }

    [Fact]
    public void Write_FileAlreadyExists_ReplacesItsContent()
    {
        var path = _folder.File("sorted.txt");
        File.WriteAllLines(path, new[] { "Old Name", "Another Old Name", "Third Old Name" });

        new FileNameOutput(path).Write(new[] { Name("Marin", "Alvarez") });

        Assert.Equal(new[] { "Marin Alvarez" }, File.ReadAllLines(path));
    }

    private static PersonName Name(string givenNames, string lastName) =>
        new(givenNames.Split(' '), lastName);
}
