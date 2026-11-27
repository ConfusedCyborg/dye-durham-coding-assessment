using DyeDurham.CodingAssessment.Domain;
using DyeDurham.CodingAssessment.Parsing;

namespace DyeDurham.CodingAssessment.Tests.Parsing;

public class NameParserTests
{
    private readonly NameParser _parser = new();

    [Fact]
    public void Parse_ThreeGivenNames_UsesLastWordAsLastName()
    {
        IReadOnlyList<PersonName> names = _parser.Parse(new[] { "Hunter Uriah Mathew Clarke" });

        PersonName name = Assert.Single(names);
        Assert.Equal(new[] { "Hunter", "Uriah", "Mathew" }, name.GivenNames);
        Assert.Equal("Clarke", name.LastName);
    }

    [Fact]
    public void Parse_OneGivenName_ReturnsGivenAndLastName()
    {
        PersonName name = Assert.Single(_parser.Parse(new[] { "Janet Parsons" }));

        Assert.Equal(new[] { "Janet" }, name.GivenNames);
        Assert.Equal("Parsons", name.LastName);
    }

    [Fact]
    public void Parse_ExtraSpaces_IgnoresThem()
    {
        PersonName name = Assert.Single(_parser.Parse(new[] { "  Adonis   Julius  Archer  " }));

        Assert.Equal("Adonis Julius Archer", name.FullName);
    }

    [Fact]
    public void Parse_TabBetweenWords_TreatsTabAsSeparator()
    {
        PersonName name = Assert.Single(_parser.Parse(new[] { "Janet\tParsons" }));

        Assert.Equal("Janet Parsons", name.FullName);
    }

    [Fact]
    public void Parse_BlankLines_SkipsThem()
    {
        IReadOnlyList<PersonName> names = _parser.Parse(new[] { "Janet Parsons", "", "   ", "Vaughn Lewis" });

        Assert.Equal(new[] { "Janet Parsons", "Vaughn Lewis" }, names.Select(n => n.FullName));
    }

    [Fact]
    public void Parse_NoGivenName_ThrowsWithLineNumber()
    {
        string[] lines = new[] { "Janet Parsons", "", "Madonna" };

        InvalidNameException exception = Assert.Throws<InvalidNameException>(() => _parser.Parse(lines));

        Assert.Equal(3, exception.LineNumber);
        Assert.Contains("Madonna", exception.Message);
    }

    [Fact]
    public void Parse_FourGivenNames_ThrowsWithLineNumber()
    {
        string[] lines = new[] { "One Two Three Four Last" };

        InvalidNameException exception = Assert.Throws<InvalidNameException>(() => _parser.Parse(lines));

        Assert.Equal(1, exception.LineNumber);
    }
}
