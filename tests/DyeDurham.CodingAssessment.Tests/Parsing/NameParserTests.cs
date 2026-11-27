using DyeDurham.CodingAssessment.Domain;
using DyeDurham.CodingAssessment.Parsing;

namespace DyeDurham.CodingAssessment.Tests.Parsing;

public class NameParserTests
{
    private readonly NameParser _parser = new();

    [Theory]
    [InlineData("Janet Parsons", "Janet", "Parsons")]
    [InlineData("Adonis Julius Archer", "Adonis Julius", "Archer")]
    [InlineData("Hunter Uriah Mathew Clarke", "Hunter Uriah Mathew", "Clarke")]
    [InlineData("  Adonis   Julius  Archer  ", "Adonis Julius", "Archer")]
    [InlineData("Janet\tParsons", "Janet", "Parsons")]
    public void Parse_ValidLine_UsesLastWordAsLastNameAndTheRestAsGivenNames(
        string line, string expectedGivenNames, string expectedLastName)
    {
        PersonName name = Assert.Single(_parser.Parse(new[] { line }));

        Assert.Equal(expectedGivenNames.Split(' '), name.GivenNames);
        Assert.Equal(expectedLastName, name.LastName);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t")]
    public void Parse_BlankLine_SkipsIt(string blankLine)
    {
        IReadOnlyList<PersonName> names = _parser.Parse(new[] { "Janet Parsons", blankLine, "Vaughn Lewis" });

        Assert.Equal(new[] { "Janet Parsons", "Vaughn Lewis" }, names.Select(name => name.FullName));
    }

    [Theory]
    [InlineData("Madonna")]
    [InlineData("  Cher  ")]
    [InlineData("One Two Three Four Last")]
    public void Parse_InvalidLine_ThrowsWithLineNumberCountingBlankLines(string invalidLine)
    {
        string[] lines = new[] { "Janet Parsons", "", invalidLine };

        InvalidNameException exception = Assert.Throws<InvalidNameException>(() => _parser.Parse(lines));

        InvalidLine reported = Assert.Single(exception.InvalidLines);
        Assert.Equal(3, reported.LineNumber);
        Assert.Equal(invalidLine.Trim(), reported.Text);
    }

    public static TheoryData<string[], int[]> LinesWithSeveralInvalidNames => new()
    {
        { new[] { "Madonna", "Janet Parsons", "Cher" }, new[] { 1, 3 } },
        { new[] { "Janet Parsons", "", "Madonna", "One Two Three Four Last" }, new[] { 3, 4 } },
        { new[] { "Madonna", "Cher", "Prince" }, new[] { 1, 2, 3 } },
    };

    [Theory]
    [MemberData(nameof(LinesWithSeveralInvalidNames))]
    public void Parse_SeveralInvalidLines_ReportsEveryOneInOrder(string[] lines, int[] expectedLineNumbers)
    {
        InvalidNameException exception = Assert.Throws<InvalidNameException>(() => _parser.Parse(lines));

        Assert.Equal(expectedLineNumbers, exception.InvalidLines.Select(line => line.LineNumber));
    }
}
