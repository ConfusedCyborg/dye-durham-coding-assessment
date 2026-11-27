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
            .ThenBy(name => name.GivenNameAt(0), IgnoringCase)
            .ThenBy(name => name.GivenNameAt(1), IgnoringCase)
            .ThenBy(name => name.GivenNameAt(2), IgnoringCase)
            // Names equal apart from case are ordered by their exact characters, so the order never changes between runs.
            .ThenBy(name => name.FullName, StringComparer.Ordinal)
            .ToList();
}
