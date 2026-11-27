using DyeDurham.CodingAssessment.Domain;

namespace DyeDurham.CodingAssessment.Parsing;

/// <summary>
/// Reads one name per line: the last word is the last name, the words before it are the given names.
/// Blank lines are skipped. Extra whitespace between words (spaces, tabs, non-breaking spaces) is ignored.
/// Every line is checked before anything is returned, so all invalid lines are reported together.
/// </summary>
public sealed class NameParser : INameParser
{
    // string.Split treats a null separator list as "any whitespace": spaces, tabs, non-breaking spaces and so on.
    private static readonly char[]? AnyWhitespace = null;

    public IReadOnlyList<PersonName> Parse(IEnumerable<string> lines)
    {
        List<NameLine> nameLines = lines
            .Select((text, index) => new NameLine(LineNumber: index + 1, Text: text.Trim()))
            .Where(line => !line.IsBlank)
            .ToList();

        List<InvalidLine> invalidLines = nameLines
            .Where(line => !line.HasValidGivenNameCount)
            .Select(line => new InvalidLine(line.LineNumber, line.Text))
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
    private sealed record NameLine(int LineNumber, string Text)
    {
        private string[] Words { get; } = Text.Split(AnyWhitespace, StringSplitOptions.RemoveEmptyEntries);

        public bool IsBlank => Words.Length == 0;

        // PersonName checks this rule too; it is checked here first so the error can say which line is wrong.
        public bool HasValidGivenNameCount => PersonName.IsValidGivenNameCount(Words.Length - 1);

        public PersonName ToPersonName() => new(givenNames: Words.SkipLast(1), lastName: Words.Last());
    }
}
