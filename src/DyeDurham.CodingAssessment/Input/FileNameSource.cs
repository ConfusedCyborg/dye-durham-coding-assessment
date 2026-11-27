namespace DyeDurham.CodingAssessment.Input;

/// <summary>
/// Reads the unsorted names from a text file.
/// </summary>
public sealed class FileNameSource : INameSource
{
    private readonly string _filePath;

    public FileNameSource(string filePath)
    {
        _filePath = filePath;
    }

    /// <exception cref="FileNotFoundException">The file does not exist.</exception>
    public IEnumerable<string> ReadLines() => File.ReadAllLines(_filePath);
}
