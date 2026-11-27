using DyeDurham.CodingAssessment.Domain;

namespace DyeDurham.CodingAssessment.Parsing;

/// <summary>
/// Reads one name per line: the last word is the last name, the words before it are the given names.
/// Blank lines are skipped. Extra spaces or tabs between words are ignored.
/// Every line is checked before anything is returned, so all invalid lines are reported together.
/// </summary>
public sealed class NameParser : INameParser
{
    private static readonly char[] WordSeparators = { ' ', '\t' };

    public IReadOnlyList<PersonName> Parse(IEnumerable<string> lines)
    {
        List<NameLine> nameLines = lines
            .Select((text, index) => new NameLine(Number: index + 1, Text: text.Trim()))
            .Where(line => !line.IsBlank)
            .ToList();

        List<InvalidLine> invalidLines = nameLines
            .Where(line => !line.HasValidGivenNameCount)
            .Select(line => new InvalidLine(line.Number, line.Text))
            .ToList();

        if (invalidLines.Count > 0)
        {
            throw new InvalidNameException(invalidLines);
        }

        return nameLines
            .Select(line => line.ToPersonName())
            .ToList();
    }

    /// <summary>
    /// One line of input with its line number, split into words.
    /// </summary>
    private sealed record NameLine(int Number, string Text)
    {
        private string[] Words { get; } = Text.Split(WordSeparators, StringSplitOptions.RemoveEmptyEntries);

        public bool IsBlank => Words.Length == 0;

        // PersonName checks this rule too; it is checked here first so the error can say which line is wrong.
        public bool HasValidGivenNameCount => PersonName.IsValidGivenNameCount(Words.Length - 1);

        public PersonName ToPersonName() => new(givenNames: Words.SkipLast(1), lastName: Words.Last());
    }
}
