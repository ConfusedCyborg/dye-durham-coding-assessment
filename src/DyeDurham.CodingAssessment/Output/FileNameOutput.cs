using DyeDurham.CodingAssessment.Domain;

namespace DyeDurham.CodingAssessment.Output;

/// <summary>
/// Writes the names to a text file, one per line. An existing file is overwritten.
/// </summary>
public sealed class FileNameOutput : INameOutput
{
    private readonly string _filePath;

    public FileNameOutput(string filePath)
    {
        _filePath = filePath;
    }

    public void Write(IReadOnlyList<PersonName> names) =>
        File.WriteAllLines(_filePath, names.Select(name => name.FullName));
}
