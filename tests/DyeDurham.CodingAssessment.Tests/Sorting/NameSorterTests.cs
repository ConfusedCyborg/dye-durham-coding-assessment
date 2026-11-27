using DyeDurham.CodingAssessment.Domain;
using DyeDurham.CodingAssessment.Sorting;

namespace DyeDurham.CodingAssessment.Tests.Sorting;

public class NameSorterTests
{
    private readonly NameSorter _sorter = new();

    [Fact]
    public void Sorts_the_example_from_the_brief_into_the_expected_order()
    {
        var sorted = Sort(
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
    public void Sorts_by_last_name_first()
    {
        Assert.Equal(new[] { "Zoe Alvarez", "Adam Bentley" }, Sort("Adam Bentley", "Zoe Alvarez"));
    }

    [Fact]
    public void Sorts_by_given_names_in_order_when_last_names_match()
    {
        Assert.Equal(new[] { "Adam John Smith", "Adam Peter Smith" }, Sort("Adam Peter Smith", "Adam John Smith"));
    }

    [Fact]
    public void Compares_the_first_given_name_before_the_second()
    {
        Assert.Equal(new[] { "Adam Zed Smith", "Beth Ann Smith" }, Sort("Beth Ann Smith", "Adam Zed Smith"));
    }

    [Fact]
    public void Puts_the_name_with_fewer_given_names_first_when_the_shared_ones_match()
    {
        Assert.Equal(new[] { "Adam Smith", "Adam John Smith" }, Sort("Adam John Smith", "Adam Smith"));
    }

    [Fact]
    public void Ignores_case()
    {
        // A case-sensitive sort would put "Baker" first, because upper case letters come before lower case ones.
        Assert.Equal(new[] { "adam archer", "Brian Baker" }, Sort("Brian Baker", "adam archer"));
    }

    [Fact]
    public void Lets_given_names_decide_before_case_does()
    {
        Assert.Equal(new[] { "adam smith", "Zoe Smith" }, Sort("Zoe Smith", "adam smith"));
    }

    [Fact]
    public void Sorts_accented_letters_next_to_their_plain_letters()
    {
        Assert.Equal(new[] { "Zoe Émond", "Adam Fisher" }, Sort("Adam Fisher", "Zoe Émond"));
    }

    [Fact]
    public void Puts_upper_case_first_when_names_differ_only_by_case()
    {
        Assert.Equal(new[] { "Adam Smith", "adam smith" }, Sort("adam smith", "Adam Smith"));
    }

    [Fact]
    public void Keeps_duplicate_names()
    {
        Assert.Equal(
            new[] { "Marin Alvarez", "Leo Gardner", "Leo Gardner" },
            Sort("Leo Gardner", "Marin Alvarez", "Leo Gardner"));
    }

    [Fact]
    public void Does_not_change_the_list_it_was_given()
    {
        var unsorted = Names("Janet Parsons", "Marin Alvarez");

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
