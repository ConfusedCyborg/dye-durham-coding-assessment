using DyeDurham.CodingAssessment.Domain;
using DyeDurham.CodingAssessment.IO;

namespace DyeDurham.CodingAssessment.Tests.IO;

public class ConsoleNameOutputTests
{
    [Fact]
    public void Write_TwoNames_PrintsOneFullNamePerLine()
    {
        StringWriter screen = new();
        ConsoleNameOutput output = new(screen);

        output.Write(new[]
        {
            new PersonName(new[] { "Marin" }, "Alvarez"),
            new PersonName(new[] { "Adonis", "Julius" }, "Archer"),
        });

        string expected = "Marin Alvarez" + Environment.NewLine + "Adonis Julius Archer" + Environment.NewLine;
        Assert.Equal(expected, screen.ToString());
    }
}
