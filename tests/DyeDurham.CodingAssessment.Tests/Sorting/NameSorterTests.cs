using DyeDurham.CodingAssessment.Domain;
using DyeDurham.CodingAssessment.Sorting;

namespace DyeDurham.CodingAssessment.Tests.Sorting;

public class NameSorterTests
{
    private readonly NameSorter _sorter = new();

    public static TheoryData<string[], string[]> NameLists => new()
    {
        // The example from the brief.
        {
            new[]
            {
                "Janet Parsons", "Vaughn Lewis", "Adonis Julius Archer", "Shelby Nathan Yoder", "Marin Alvarez",
                "London Lindsey", "Beau Tristan Bentley", "Leo Gardner", "Hunter Uriah Mathew Clarke",
                "Mikayla Lopez", "Frankie Conner Ritter",
            },
            new[]
            {
                "Marin Alvarez", "Adonis Julius Archer", "Beau Tristan Bentley", "Hunter Uriah Mathew Clarke",
                "Leo Gardner", "Vaughn Lewis", "London Lindsey", "Mikayla Lopez", "Janet Parsons",
                "Frankie Conner Ritter", "Shelby Nathan Yoder",
            }
        },
        // Duplicates are kept.
        {
            new[] { "Leo Gardner", "Marin Alvarez", "Leo Gardner" },
            new[] { "Marin Alvarez", "Leo Gardner", "Leo Gardner" }
        },
        // An empty list stays empty.
        {
            Array.Empty<string>(),
            Array.Empty<string>()
        },
    };

    [Theory]
    [MemberData(nameof(NameLists))]
    public void Sort_NameList_OrdersByLastNameThenGivenNames(string[] unsorted, string[] expected)
    {
        Assert.Equal(expected, Sort(unsorted));
    }

    [Theory]
    [InlineData("Zoe Alvarez", "Adam Bentley")]                  // Last name decides first.
    [InlineData("Adam John Smith", "Adam Peter Smith")]          // Same last name: given names decide.
    [InlineData("Adam Zed Smith", "Beth Ann Smith")]             // The first given name decides before the second.
    [InlineData("Adam Smith", "Adam John Smith")]                // Shared given names match: fewer given names first.
    [InlineData("Adam John Smith", "Adam John Brian Smith")]     // Same rule at the third given name.
    [InlineData("adam archer", "Brian Baker")]                   // Case is ignored ("Baker" would come first otherwise).
    [InlineData("Adam bob Smith", "Adam Carl Smith")]            // Case is ignored on the second given name.
    [InlineData("Adam John carl Smith", "Adam John Dave Smith")] // Case is ignored on the third given name.
    [InlineData("adam smith", "Zoe Smith")]                      // Given names decide before case does.
    [InlineData("Zoe Émond", "Adam Fisher")]                     // Accented letters sort next to their plain letters.
    [InlineData("Adam Smith", "adam smith")]                     // Equal apart from case: upper case first.
    public void Sort_TwoNamesInEitherOrder_PutsThemInRuleOrder(string expectedFirst, string expectedSecond)
    {
        string[] expected = new[] { expectedFirst, expectedSecond };

        Assert.Equal(expected, Sort(expectedSecond, expectedFirst));
        Assert.Equal(expected, Sort(expectedFirst, expectedSecond));
    }

    [Fact]
    public void Sort_AnyList_LeavesInputUnchanged()
    {
        List<PersonName> unsorted = TestNames.From("Janet Parsons", "Marin Alvarez");

        _sorter.Sort(unsorted);

        Assert.Equal(new[] { "Janet Parsons", "Marin Alvarez" }, unsorted.Select(name => name.FullName));
    }

    private string[] Sort(params string[] fullNames) =>
        _sorter.Sort(TestNames.From(fullNames)).Select(name => name.FullName).ToArray();
}
