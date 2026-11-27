namespace DyeDurham.CodingAssessment.Parsing;

/// <summary>
/// A line of input that is not a valid name.
/// </summary>
/// <param name="LineNumber">The 1-based line number in the input, counting blank lines.</param>
/// <param name="Text">The line without its leading and trailing spaces.</param>
public sealed record InvalidLine(int LineNumber, string Text);
