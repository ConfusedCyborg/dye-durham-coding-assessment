using DyeDurham.CodingAssessment.Domain;
using DyeDurham.CodingAssessment.Sorting;

namespace DyeDurham.CodingAssessment.Tests.Sorting;

public class PersonNameComparerTests
{
    private readonly PersonNameComparer _comparer = new();

    [Fact]
    public void Compares_last_names_first()
    {
        var alvarez = Name("Zoe", "Alvarez");
        var bentley = Name("Adam", "Bentley");

        Assert.True(_comparer.Compare(alvarez, bentley) < 0);
        Assert.True(_comparer.Compare(bentley, alvarez) > 0);
    }

    [Fact]
    public void Compares_given_names_in_order_when_last_names_match()
    {
        var adamJohnSmith = Name("Adam John", "Smith");
        var adamPeterSmith = Name("Adam Peter", "Smith");

        Assert.True(_comparer.Compare(adamJohnSmith, adamPeterSmith) < 0);
    }

    [Fact]
    public void Compares_the_first_given_name_before_the_second()
    {
        var adamZedSmith = Name("Adam Zed", "Smith");
        var bethAnnSmith = Name("Beth Ann", "Smith");

        Assert.True(_comparer.Compare(adamZedSmith, bethAnnSmith) < 0);
    }

    [Fact]
    public void Puts_the_name_with_fewer_given_names_first_when_the_shared_ones_match()
    {
        var adamSmith = Name("Adam", "Smith");
        var adamJohnSmith = Name("Adam John", "Smith");

        Assert.True(_comparer.Compare(adamSmith, adamJohnSmith) < 0);
    }

    [Fact]
    public void Ignores_case_when_comparing()
    {
        var lowerCase = Name("adam", "baker");
        var upperCase = Name("Brian", "Archer");

        Assert.True(_comparer.Compare(upperCase, lowerCase) < 0);
    }

    [Fact]
    public void Sorts_accented_letters_next_to_their_plain_letters()
    {
        var emond = Name("Zoe", "Émond");
        var fisher = Name("Adam", "Fisher");

        Assert.True(_comparer.Compare(emond, fisher) < 0);
    }

    [Fact]
    public void Orders_names_that_differ_only_by_case_the_same_way_every_time()
    {
        var upperCase = Name("Adam", "Smith");
        var lowerCase = Name("adam", "smith");

        Assert.True(_comparer.Compare(upperCase, lowerCase) < 0);
        Assert.True(_comparer.Compare(lowerCase, upperCase) > 0);
    }

    [Fact]
    public void Treats_identical_names_as_equal()
    {
        Assert.Equal(0, _comparer.Compare(Name("Adam", "Smith"), Name("Adam", "Smith")));
    }

    private static PersonName Name(string givenNames, string lastName) =>
        new(givenNames.Split(' '), lastName);
}
