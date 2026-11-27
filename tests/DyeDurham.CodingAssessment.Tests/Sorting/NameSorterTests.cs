using DyeDurham.CodingAssessment.Domain;
using DyeDurham.CodingAssessment.Sorting;

namespace DyeDurham.CodingAssessment.Tests.Sorting;

public class NameSorterTests
{
    private readonly NameSorter _sorter = new();

    [Fact]
    public void Sort_BriefExample_ReturnsExpectedOrder()
    {
        string[] sorted = Sort(
            "Janet Parsons",
            "Vaughn Lewis",
            "Adonis Julius Archer",
            "Shelby Nathan Yoder",
            "Marin Alvarez",
            "London Lindsey",
            "Beau Tristan Bentley",
            "Leo Gardner",
            "Hunter Uriah Mathew Clarke",
            "Mikayla Lopez",
            "Frankie Conner Ritter");

        Assert.Equal(
            new[]
            {
                "Marin Alvarez",
                "Adonis Julius Archer",
                "Beau Tristan Bentley",
                "Hunter Uriah Mathew Clarke",
                "Leo Gardner",
                "Vaughn Lewis",
                "London Lindsey",
                "Mikayla Lopez",
                "Janet Parsons",
                "Frankie Conner Ritter",
                "Shelby Nathan Yoder",
            },
            sorted);
    }

    [Fact]
    public void Sort_DifferentLastNames_OrdersByLastName()
    {
        Assert.Equal(new[] { "Zoe Alvarez", "Adam Bentley" }, Sort("Adam Bentley", "Zoe Alvarez"));
    }

    [Fact]
    public void Sort_SameLastName_OrdersByGivenNames()
    {
        Assert.Equal(new[] { "Adam John Smith", "Adam Peter Smith" }, Sort("Adam Peter Smith", "Adam John Smith"));
    }

    [Fact]
    public void Sort_DifferentFirstGivenNames_IgnoresSecondGivenName()
    {
        Assert.Equal(new[] { "Adam Zed Smith", "Beth Ann Smith" }, Sort("Beth Ann Smith", "Adam Zed Smith"));
    }

    [Fact]
    public void Sort_SharedGivenNamesMatch_PutsFewerGivenNamesFirst()
    {
        Assert.Equal(new[] { "Adam Smith", "Adam John Smith" }, Sort("Adam John Smith", "Adam Smith"));
    }

    [Fact]
    public void Sort_MixedCase_IgnoresCase()
    {
        // A case-sensitive sort would put "Baker" first, because upper case letters come before lower case ones.
        Assert.Equal(new[] { "adam archer", "Brian Baker" }, Sort("Brian Baker", "adam archer"));
    }

    [Fact]
    public void Sort_LastNamesDifferOnlyByCase_OrdersByGivenNames()
    {
        Assert.Equal(new[] { "adam smith", "Zoe Smith" }, Sort("Zoe Smith", "adam smith"));
    }

    [Fact]
    public void Sort_AccentedLastName_SortsNextToPlainLetter()
    {
        Assert.Equal(new[] { "Zoe Émond", "Adam Fisher" }, Sort("Adam Fisher", "Zoe Émond"));
    }

    [Fact]
    public void Sort_NamesDifferOnlyByCase_PutsUpperCaseFirst()
    {
        Assert.Equal(new[] { "Adam Smith", "adam smith" }, Sort("adam smith", "Adam Smith"));
    }

    [Fact]
    public void Sort_DuplicateNames_KeepsBoth()
    {
        Assert.Equal(
            new[] { "Marin Alvarez", "Leo Gardner", "Leo Gardner" },
            Sort("Leo Gardner", "Marin Alvarez", "Leo Gardner"));
    }

    [Fact]
    public void Sort_AnyList_LeavesInputUnchanged()
    {
        List<PersonName> unsorted = Names("Janet Parsons", "Marin Alvarez");

        _sorter.Sort(unsorted);

        Assert.Equal(new[] { "Janet Parsons", "Marin Alvarez" }, unsorted.Select(name => name.FullName));
    }

    private string[] Sort(params string[] fullNames) =>
        _sorter.Sort(Names(fullNames)).Select(name => name.FullName).ToArray();

    private static List<PersonName> Names(params string[] fullNames) =>
        fullNames
            .Select(fullName => fullName.Split(' '))
            .Select(words => new PersonName(words[..^1], words[^1]))
            .ToList();
}
