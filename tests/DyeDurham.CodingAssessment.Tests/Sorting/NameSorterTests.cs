using DyeDurham.CodingAssessment.Domain;
using DyeDurham.CodingAssessment.Sorting;

namespace DyeDurham.CodingAssessment.Tests.Sorting;

public class NameSorterTests
{
    [Fact]
    public void Sorts_the_example_from_the_brief_into_the_expected_order()
    {
        var unsorted = Names(
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

        var sorter = new NameSorter(new PersonNameComparer());

        var sorted = sorter.Sort(unsorted);

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
            sorted.Select(name => name.FullName));
    }

    [Fact]
    public void Keeps_duplicate_names()
    {
        var sorter = new NameSorter(new PersonNameComparer());

        var sorted = sorter.Sort(Names("Leo Gardner", "Marin Alvarez", "Leo Gardner"));

        Assert.Equal(new[] { "Marin Alvarez", "Leo Gardner", "Leo Gardner" }, sorted.Select(name => name.FullName));
    }

    [Fact]
    public void Does_not_change_the_list_it_was_given()
    {
        var unsorted = Names("Janet Parsons", "Marin Alvarez");
        var sorter = new NameSorter(new PersonNameComparer());

        sorter.Sort(unsorted);

        Assert.Equal(new[] { "Janet Parsons", "Marin Alvarez" }, unsorted.Select(name => name.FullName));
    }

    private static List<PersonName> Names(params string[] fullNames) =>
        fullNames
            .Select(fullName => fullName.Split(' '))
            .Select(words => new PersonName(words[..^1], words[^1]))
            .ToList();
}
