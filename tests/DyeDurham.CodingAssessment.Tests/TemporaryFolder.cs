namespace DyeDurham.CodingAssessment.Tests;

/// <summary>
/// A fresh folder under the system temp directory, deleted when the test finishes.
/// </summary>
public sealed class TemporaryFolder : IDisposable
{
    public string Path { get; } = Directory.CreateTempSubdirectory("name-sorter-tests-").FullName;

    public string File(string fileName) => System.IO.Path.Combine(Path, fileName);

    public void Dispose()
    {
        // Some tests make a file read-only, which would stop the folder from being deleted on Windows.
        foreach (var file in Directory.EnumerateFiles(Path))
        {
            System.IO.File.SetAttributes(file, FileAttributes.Normal);
        }

        Directory.Delete(Path, recursive: true);
    }
}
