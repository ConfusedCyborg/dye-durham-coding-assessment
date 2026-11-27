namespace DyeDurham.CodingAssessment.Input;

/// <summary>
/// Where the unsorted names come from, one name per line.
/// </summary>
public interface INameSource
{
    /// <summary>Returns every line of the source, including blank ones, without line endings.</summary>
    IEnumerable<string> ReadLines();
}
