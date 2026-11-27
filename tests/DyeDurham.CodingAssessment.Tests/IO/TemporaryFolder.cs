namespace DyeDurham.CodingAssessment.Tests.IO;

/// <summary>
/// A fresh folder under the system temp directory, deleted when the test finishes.
/// </summary>
public sealed class TemporaryFolder : IDisposable
{
    public string Path { get; } = Directory.CreateTempSubdirectory("name-sorter-tests-").FullName;

    public string File(string fileName) => System.IO.Path.Combine(Path, fileName);

    public void Dispose() => Directory.Delete(Path, recursive: true);
}
