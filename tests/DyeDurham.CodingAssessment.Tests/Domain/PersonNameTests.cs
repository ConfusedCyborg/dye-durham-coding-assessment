using DyeDurham.CodingAssessment.Domain;

namespace DyeDurham.CodingAssessment.Tests.Domain;

public class PersonNameTests
{
    [Fact]
    public void FullName_ThreeGivenNames_JoinsAllPartsWithSpaces()
    {
        PersonName name = new(new[] { "Hunter", "Uriah", "Mathew" }, "Clarke");

        Assert.Equal("Hunter Uriah Mathew Clarke", name.FullName);
    }

    [Theory]
    [InlineData(1)]
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
    public void Constructor_ZeroOrFourGivenNames_ThrowsArgumentException(int givenNameCount)
    {
        string[] givenNames = Enumerable.Repeat("Given", givenNameCount).ToArray();

        Assert.Throws<ArgumentException>(() => new PersonName(givenNames, "Last"));
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(3, true)]
    [InlineData(4, false)]
    public void IsValidGivenNameCount_Count_ReturnsWhetherOneToThree(int givenNameCount, bool expected)
    {
        Assert.Equal(expected, PersonName.IsValidGivenNameCount(givenNameCount));
    }

    [Fact]
    public void GivenNameAt_ExistingPosition_ReturnsGivenName()
    {
        PersonName name = new(new[] { "Adonis", "Julius" }, "Archer");

        Assert.Equal("Adonis", name.GivenNameAt(0));
        Assert.Equal("Julius", name.GivenNameAt(1));
    }

    [Fact]
    public void GivenNameAt_PositionPastLastGivenName_ReturnsNull()
    {
        PersonName name = new(new[] { "Adonis", "Julius" }, "Archer");

        Assert.Null(name.GivenNameAt(2));
    }

    [Fact]
    public void Constructor_InputArrayChangedAfterwards_KeepsOriginalGivenNames()
    {
        string[] givenNames = new[] { "Janet" };
        PersonName name = new(givenNames, "Parsons");

        givenNames[0] = "Changed";

        Assert.Equal("Janet", name.GivenNames[0]);
    }
}
