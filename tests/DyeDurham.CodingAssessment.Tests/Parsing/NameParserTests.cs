using DyeDurham.CodingAssessment.Parsing;

namespace DyeDurham.CodingAssessment.Tests.Parsing;

public class NameParserTests
{
    private readonly NameParser _parser = new();

    [Fact]
    public void Parse_ThreeGivenNames_UsesLastWordAsLastName()
    {
        var names = _parser.Parse(new[] { "Hunter Uriah Mathew Clarke" });

        var name = Assert.Single(names);
        Assert.Equal(new[] { "Hunter", "Uriah", "Mathew" }, name.GivenNames);
        Assert.Equal("Clarke", name.LastName);
    }

    [Fact]
    public void Parse_OneGivenName_ReturnsGivenAndLastName()
    {
        var name = Assert.Single(_parser.Parse(new[] { "Janet Parsons" }));

        Assert.Equal(new[] { "Janet" }, name.GivenNames);
        Assert.Equal("Parsons", name.LastName);
    }

    [Fact]
    public void Parse_ExtraSpaces_IgnoresThem()
    {
        var name = Assert.Single(_parser.Parse(new[] { "  Adonis   Julius  Archer  " }));

        Assert.Equal("Adonis Julius Archer", name.FullName);
    }

    [Fact]
    public void Parse_TabBetweenWords_TreatsTabAsSeparator()
    {
        var name = Assert.Single(_parser.Parse(new[] { "Janet\tParsons" }));

        Assert.Equal("Janet Parsons", name.FullName);
    }

    [Fact]
    public void Parse_BlankLines_SkipsThem()
    {
        var names = _parser.Parse(new[] { "Janet Parsons", "", "   ", "Vaughn Lewis" });

        Assert.Equal(new[] { "Janet Parsons", "Vaughn Lewis" }, names.Select(n => n.FullName));
    }

    [Fact]
    public void Parse_NoGivenName_ThrowsWithLineNumber()
    {
        var lines = new[] { "Janet Parsons", "", "Madonna" };

        var exception = Assert.Throws<InvalidNameException>(() => _parser.Parse(lines));

        Assert.Equal(3, exception.LineNumber);
        Assert.Contains("Madonna", exception.Message);
    }

    [Fact]
    public void Parse_FourGivenNames_ThrowsWithLineNumber()
    {
        var lines = new[] { "One Two Three Four Last" };

        var exception = Assert.Throws<InvalidNameException>(() => _parser.Parse(lines));

        Assert.Equal(1, exception.LineNumber);
    }
}
