using DyeDurham.CodingAssessment.Domain;

namespace DyeDurham.CodingAssessment.Parsing;

/// <summary>
/// Turns raw text lines into person names.
/// </summary>
public interface INameParser
{
    /// <exception cref="InvalidNameException">A line does not hold a valid name.</exception>
    IReadOnlyList<PersonName> Parse(IEnumerable<string> lines);
}
