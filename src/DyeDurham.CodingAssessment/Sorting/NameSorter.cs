using DyeDurham.CodingAssessment.Domain;

namespace DyeDurham.CodingAssessment.Sorting;

/// <summary>
/// Sorts names by last name, then by the first, second and third given name.
/// A name with fewer given names comes first when the others match, because a missing given name is null.
/// Case is ignored and accented letters sort next to their plain letters (invariant culture).
/// </summary>
public sealed class NameSorter : INameSorter
{
    private static readonly StringComparer IgnoringCase = StringComparer.InvariantCultureIgnoreCase;

    public IReadOnlyList<PersonName> Sort(IEnumerable<PersonName> names) =>
        names
            .OrderBy(name => name.LastName, IgnoringCase)
            // One step per given name position, up to PersonName.MaxGivenNames (3).
            .ThenBy(name => name.GivenNameAt(0), IgnoringCase)
            .ThenBy(name => name.GivenNameAt(1), IgnoringCase)
            .ThenBy(name => name.GivenNameAt(2), IgnoringCase)
            // Names equal apart from case ("Adam Smith", "adam smith") always come out upper case first,
            // whatever order the file had them in.
            .ThenBy(name => name.FullName, StringComparer.Ordinal)
            .ToList();
}
