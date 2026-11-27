namespace DyeDurham.CodingAssessment.IO;

/// <summary>
/// Where the unsorted names come from, one name per line.
/// </summary>
public interface INameSource
{
    IEnumerable<string> ReadLines();
}
