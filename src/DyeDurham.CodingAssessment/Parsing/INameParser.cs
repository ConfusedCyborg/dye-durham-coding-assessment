using DyeDurham.CodingAssessment.Domain;

namespace DyeDurham.CodingAssessment.Parsing;

/// <summary>
/// Turns raw text lines into person names.
/// </summary>
public interface INameParser
{
    /// <summary>Returns one name per non-blank line, in the same order as the lines.</summary>
    /// <exception cref="InvalidNameException">One or more lines do not hold a valid name; all of them are listed.</exception>
    IReadOnlyList<PersonName> Parse(IEnumerable<string> lines);
}
