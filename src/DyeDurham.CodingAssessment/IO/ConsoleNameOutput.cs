using DyeDurham.CodingAssessment.Domain;

namespace DyeDurham.CodingAssessment.IO;

/// <summary>
/// Prints the names to the screen, one per line.
/// Takes a <see cref="TextWriter"/> so tests can capture what would be printed; the app passes <see cref="Console.Out"/>.
/// </summary>
public sealed class ConsoleNameOutput : INameOutput
{
    private readonly TextWriter _screen;

    public ConsoleNameOutput(TextWriter screen)
    {
        _screen = screen;
    }

    public void Write(IReadOnlyList<PersonName> names)
    {
        foreach (var name in names)
        {
            _screen.WriteLine(name.FullName);
        }
    }
}
