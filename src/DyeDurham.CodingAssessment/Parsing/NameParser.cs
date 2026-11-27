using DyeDurham.CodingAssessment.Domain;

namespace DyeDurham.CodingAssessment.Parsing;

/// <summary>
/// Reads one name per line: the last word is the last name, the words before it are the given names.
/// Blank lines are skipped. Extra spaces or tabs between words are ignored.
/// </summary>
public sealed class NameParser : INameParser
{
    private static readonly char[] WordSeparators = { ' ', '\t' };

    public IReadOnlyList<PersonName> Parse(IEnumerable<string> lines) =>
        lines
            .Select((text, index) => (Text: text, Number: index + 1))
            .Where(line => !string.IsNullOrWhiteSpace(line.Text))
            .Select(line => ParseLine(line.Text, line.Number))
            .ToList();

    private static PersonName ParseLine(string line, int lineNumber)
    {
        var words = line.Split(WordSeparators, StringSplitOptions.RemoveEmptyEntries);
        var givenNames = words.SkipLast(1).ToArray();
        var lastName = words.Last();

        // Checked here, before PersonName checks it too, so the error can say which line is wrong.
        if (!PersonName.IsValidGivenNameCount(givenNames.Length))
        {
            throw new InvalidNameException(
                lineNumber,
                line.Trim(),
                $"expected {PersonName.MinGivenNames} to {PersonName.MaxGivenNames} given names followed by a last name.");
        }

        return new PersonName(givenNames, lastName);
    }
}
