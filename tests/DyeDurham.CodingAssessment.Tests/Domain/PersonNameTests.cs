using DyeDurham.CodingAssessment.Domain;

namespace DyeDurham.CodingAssessment.Tests.Domain;

public class PersonNameTests
{
    [Theory]
    [InlineData("Janet", "Parsons", "Janet Parsons")]
    [InlineData("Adonis Julius", "Archer", "Adonis Julius Archer")]
    [InlineData("Hunter Uriah Mathew", "Clarke", "Hunter Uriah Mathew Clarke")]
    public void FullName_GivenAndLastNames_JoinsThemWithSpaces(string givenNames, string lastName, string expected)
    {
        PersonName name = new(givenNames.Split(' '), lastName);

        Assert.Equal(expected, name.FullName);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Constructor_OneToThreeGivenNames_CreatesName(int givenNameCount)
    {
        string[] givenNames = Enumerable.Repeat("Given", givenNameCount).ToArray();

        PersonName name = new(givenNames, "Last");

        Assert.Equal(givenNameCount, name.GivenNames.Count);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    [InlineData(5)]
    public void Constructor_TooFewOrTooManyGivenNames_ThrowsArgumentException(int givenNameCount)
    {
        string[] givenNames = Enumerable.Repeat("Given", givenNameCount).ToArray();

        Assert.Throws<ArgumentException>(() => new PersonName(givenNames, "Last"));
    }

    [Fact]
    public void Constructor_InputArrayChangedAfterwards_KeepsOriginalGivenNames()
    {
        string[] givenNames = new[] { "Janet" };
        PersonName name = new(givenNames, "Parsons");

        givenNames[0] = "Changed";

        Assert.Equal("Janet", name.GivenNames[0]);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(2, true)]
    [InlineData(3, true)]
    [InlineData(0, false)]
    [InlineData(4, false)]
    [InlineData(-1, false)]
    public void IsValidGivenNameCount_VariousCounts_ReturnsTrueOnlyForOneToThree(int givenNameCount, bool expected)
    {
        Assert.Equal(expected, PersonName.IsValidGivenNameCount(givenNameCount));
    }

    [Theory]
    [InlineData(0, "Adonis")]
    [InlineData(1, "Julius")]
    [InlineData(2, null)]
    public void GivenNameAt_Position_ReturnsGivenNameOrNullWhenMissing(int position, string? expected)
    {
        PersonName name = new(new[] { "Adonis", "Julius" }, "Archer");

        Assert.Equal(expected, name.GivenNameAt(position));
    }

    [Fact]
    public void GivenNameAt_NegativePosition_ThrowsArgumentOutOfRangeException()
    {
        PersonName name = new(new[] { "Adonis", "Julius" }, "Archer");

        Assert.Throws<ArgumentOutOfRangeException>(() => name.GivenNameAt(-1));
    }
}
