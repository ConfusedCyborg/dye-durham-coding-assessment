using DyeDurham.CodingAssessment.Parsing;

namespace DyeDurham.CodingAssessment.Tests.Parsing;

public class NameParserTests
{
    private readonly NameParser _parser = new();

    [Fact]
    public void Treats_the_last_word_as_the_last_name_and_the_rest_as_given_names()
    {
        var names = _parser.Parse(new[] { "Hunter Uriah Mathew Clarke" });

        var name = Assert.Single(names);
        Assert.Equal(new[] { "Hunter", "Uriah", "Mathew" }, name.GivenNames);
        Assert.Equal("Clarke", name.LastName);
    }

    [Fact]
    public void Parses_a_name_with_a_single_given_name()
    {
        var name = Assert.Single(_parser.Parse(new[] { "Janet Parsons" }));

        Assert.Equal(new[] { "Janet" }, name.GivenNames);
        Assert.Equal("Parsons", name.LastName);
    }

    [Fact]
    public void Ignores_leading_trailing_and_repeated_spaces()
    {
        var name = Assert.Single(_parser.Parse(new[] { "  Adonis   Julius  Archer  " }));

        Assert.Equal("Adonis Julius Archer", name.FullName);
    }

    [Fact]
    public void Skips_blank_lines()
    {
        var names = _parser.Parse(new[] { "Janet Parsons", "", "   ", "Vaughn Lewis" });

        Assert.Equal(new[] { "Janet Parsons", "Vaughn Lewis" }, names.Select(n => n.FullName));
    }

    [Fact]
    public void Rejects_a_name_with_no_given_name_and_reports_its_line_number()
    {
        var lines = new[] { "Janet Parsons", "", "Madonna" };

        var exception = Assert.Throws<InvalidNameException>(() => _parser.Parse(lines));

        Assert.Equal(3, exception.LineNumber);
        Assert.Contains("Madonna", exception.Message);
    }

    [Fact]
    public void Rejects_a_name_with_more_than_three_given_names_and_reports_its_line_number()
    {
        var lines = new[] { "One Two Three Four Last" };

        var exception = Assert.Throws<InvalidNameException>(() => _parser.Parse(lines));

        Assert.Equal(1, exception.LineNumber);
    }
}
