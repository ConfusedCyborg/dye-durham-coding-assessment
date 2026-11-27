using DyeDurham.CodingAssessment.Domain;

namespace DyeDurham.CodingAssessment.Parsing;

/// <summary>
/// Reads one name per line: the last word is the last name, the words before it are the given names.
/// Blank lines are skipped. Extra spaces between words are ignored.
/// </summary>
public sealed class NameParser : INameParser
{
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
        var words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var givenNameCount = words.Length - 1;

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
