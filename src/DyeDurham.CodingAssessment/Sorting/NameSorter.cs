using DyeDurham.CodingAssessment.Domain;

namespace DyeDurham.CodingAssessment.Sorting;

/// <summary>
/// Sorts names with the comparer it is given, so the ordering rule can change without touching this class.
/// </summary>
public sealed class NameSorter : INameSorter
{
    private readonly IComparer<PersonName> _comparer;

    public NameSorter(IComparer<PersonName> comparer)
    {
        _comparer = comparer;
    }

    public IReadOnlyList<PersonName> Sort(IEnumerable<PersonName> names) =>
        names.OrderBy(name => name, _comparer).ToList();
}
