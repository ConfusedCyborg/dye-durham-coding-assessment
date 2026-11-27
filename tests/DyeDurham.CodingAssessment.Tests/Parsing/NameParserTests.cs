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

        Assert.Equal(3, exception.LineNumber);
        Assert.Contains(invalidLine.Trim(), exception.Message);
    }
}
