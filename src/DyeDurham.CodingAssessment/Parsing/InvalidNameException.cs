using DyeDurham.CodingAssessment.Domain;

namespace DyeDurham.CodingAssessment.Parsing;

/// <summary>
/// Thrown when one or more lines of input cannot be read as a person name.
/// The message lists every invalid line, so they can all be fixed in one go.
/// </summary>
public sealed class InvalidNameException : Exception
{
    public InvalidNameException(IReadOnlyList<InvalidLine> invalidLines)
        : base(DescribeEach(invalidLines))
    {
        InvalidLines = invalidLines;
    }

    public IReadOnlyList<InvalidLine> InvalidLines { get; }

    private static string DescribeEach(IEnumerable<InvalidLine> invalidLines) =>
        string.Join(
            Environment.NewLine,
            invalidLines.Select(line =>
                $"Line {line.LineNumber} (\"{line.Text}\"): " +
                $"expected {PersonName.MinGivenNames} to {PersonName.MaxGivenNames} given names followed by a last name."));
}
