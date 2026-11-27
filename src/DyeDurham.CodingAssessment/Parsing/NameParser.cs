using DyeDurham.CodingAssessment.Domain;

namespace DyeDurham.CodingAssessment.Parsing;

/// <summary>
/// Reads one name per line: the last word is the last name, the words before it are the given names.
/// Blank lines are skipped. Extra spaces or tabs between words are ignored.
/// </summary>
public sealed class NameParser : INameParser
{
    private static readonly char[] WordSeparators = { ' ', '\t' };

    public IReadOnlyList<PersonName> Parse(IEnumerable<string> lines)
    {
        var names = new List<PersonName>();
        var lineNumber = 0;

        foreach (var line in lines)
        {
            lineNumber++;

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            names.Add(ParseLine(line, lineNumber));
        }

        return names;
    }

    private static PersonName ParseLine(string line, int lineNumber)
    {
        var words = line.Split(WordSeparators, StringSplitOptions.RemoveEmptyEntries);
        var givenNameCount = words.Length - 1;

        // PersonName checks this rule too; it is checked here first so the error can name the line.
        if (givenNameCount < PersonName.MinGivenNames || givenNameCount > PersonName.MaxGivenNames)
        {
            throw new InvalidNameException(
                lineNumber,
                line.Trim(),
                $"expected {PersonName.MinGivenNames} to {PersonName.MaxGivenNames} given names followed by a last name.");
        }

        return new PersonName(givenNames: words[..^1], lastName: words[^1]);
    }
}
