using DyeDurham.CodingAssessment.IO;

namespace DyeDurham.CodingAssessment.Tests.IO;

public class ConsoleNameOutputTests
{
    public static TheoryData<string[]> NameLists => new()
    {
        Array.Empty<string>(),
        new[] { "Marin Alvarez" },
        new[] { "Marin Alvarez", "Adonis Julius Archer" },
    };

    [Theory]
    [MemberData(nameof(NameLists))]
    public void Write_Names_PrintsOneFullNamePerLine(string[] fullNames)
    {
        StringWriter screen = new();
        ConsoleNameOutput output = new(screen);

        output.Write(TestNames.From(fullNames));

        string expected = string.Concat(fullNames.Select(fullName => fullName + Environment.NewLine));
        Assert.Equal(expected, screen.ToString());
    }
}
