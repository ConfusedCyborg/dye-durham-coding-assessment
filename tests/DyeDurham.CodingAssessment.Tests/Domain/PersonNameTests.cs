using DyeDurham.CodingAssessment.Domain;

namespace DyeDurham.CodingAssessment.Tests.Domain;

public class PersonNameTests
{
    [Fact]
    public void FullName_joins_given_names_and_last_name_with_single_spaces()
    {
        var name = new PersonName(new[] { "Hunter", "Uriah", "Mathew" }, "Clarke");

        Assert.Equal("Hunter Uriah Mathew Clarke", name.FullName);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    public void Accepts_between_one_and_three_given_names(int givenNameCount)
    {
        var givenNames = Enumerable.Repeat("Given", givenNameCount).ToArray();

        var name = new PersonName(givenNames, "Last");

        Assert.Equal(givenNameCount, name.GivenNames.Count);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    public void Rejects_fewer_than_one_or_more_than_three_given_names(int givenNameCount)
    {
        var givenNames = Enumerable.Repeat("Given", givenNameCount).ToArray();

        Assert.Throws<ArgumentException>(() => new PersonName(givenNames, "Last"));
    }

    [Fact]
    public void GivenNameAt_returns_the_given_name_at_that_position()
    {
        var name = new PersonName(new[] { "Adonis", "Julius" }, "Archer");

        Assert.Equal("Adonis", name.GivenNameAt(0));
        Assert.Equal("Julius", name.GivenNameAt(1));
    }

    [Fact]
    public void GivenNameAt_returns_null_when_the_name_has_no_given_name_at_that_position()
    {
        var name = new PersonName(new[] { "Adonis", "Julius" }, "Archer");

        Assert.Null(name.GivenNameAt(2));
    }

    [Fact]
    public void Keeps_its_own_copy_of_the_given_names()
    {
        var givenNames = new[] { "Janet" };
        var name = new PersonName(givenNames, "Parsons");

        givenNames[0] = "Changed";

        Assert.Equal("Janet", name.GivenNames[0]);
    }
}
