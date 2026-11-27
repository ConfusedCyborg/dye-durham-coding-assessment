using DyeDurham.CodingAssessment.Domain;
using DyeDurham.CodingAssessment.IO;

namespace DyeDurham.CodingAssessment.Tests.IO;

public class ConsoleNameOutputTests
{
    [Fact]
    public void Writes_one_full_name_per_line()
    {
        var screen = new StringWriter();
        var output = new ConsoleNameOutput(screen);

        output.Write(new[]
        {
            new PersonName(new[] { "Marin" }, "Alvarez"),
            new PersonName(new[] { "Adonis", "Julius" }, "Archer"),
        });

        var expected = "Marin Alvarez" + Environment.NewLine + "Adonis Julius Archer" + Environment.NewLine;
        Assert.Equal(expected, screen.ToString());
    }
}
