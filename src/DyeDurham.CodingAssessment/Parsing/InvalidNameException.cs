namespace DyeDurham.CodingAssessment.Parsing;

/// <summary>
/// Thrown when a line of input cannot be read as a person name.
/// </summary>
public sealed class InvalidNameException : Exception
{
    public InvalidNameException(int lineNumber, string line, string reason)
        : base($"Line {lineNumber} (\"{line}\"): {reason}")
    {
        LineNumber = lineNumber;
    }

    /// <summary>The 1-based line number in the input, counting blank lines.</summary>
    public int LineNumber { get; }
}
