using DyeDurham.CodingAssessment.Domain;

namespace DyeDurham.CodingAssessment.Sorting;

/// <summary>
/// Puts person names in order.
/// </summary>
public interface INameSorter
{
    /// <summary>Returns a new sorted list; the input is left unchanged.</summary>
    IReadOnlyList<PersonName> Sort(IEnumerable<PersonName> names);
}
