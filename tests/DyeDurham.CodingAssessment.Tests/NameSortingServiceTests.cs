using DyeDurham.CodingAssessment.Domain;
using DyeDurham.CodingAssessment.IO;
using DyeDurham.CodingAssessment.Parsing;
using DyeDurham.CodingAssessment.Sorting;

namespace DyeDurham.CodingAssessment.Tests;

public class NameSortingServiceTests
{
    [Fact]
    public void Sends_the_same_sorted_names_to_every_output()
    {
        var source = new InMemoryNameSource("Janet Parsons", "Marin Alvarez", "Adonis Julius Archer");
        var screen = new InMemoryNameOutput();
        var file = new InMemoryNameOutput();
        var service = CreateService(source, screen, file);

        service.Run();

        var expected = new[] { "Marin Alvarez", "Adonis Julius Archer", "Janet Parsons" };
        Assert.Equal(expected, screen.WrittenNames);
        Assert.Equal(expected, file.WrittenNames);
    }

    [Fact]
    public void Writes_nothing_when_a_name_is_invalid()
    {
        var source = new InMemoryNameSource("Janet Parsons", "Madonna");
        var output = new InMemoryNameOutput();
        var service = CreateService(source, output);

        Assert.Throws<InvalidNameException>(() => service.Run());
        Assert.Empty(output.WrittenNames);
    }

    private static NameSortingService CreateService(INameSource source, params INameOutput[] outputs) =>
        new(source, new NameParser(), new NameSorter(), outputs);

    private sealed class InMemoryNameSource : INameSource
    {
        private readonly string[] _lines;

        public InMemoryNameSource(params string[] lines) => _lines = lines;

        public IEnumerable<string> ReadLines() => _lines;
    }

    private sealed class InMemoryNameOutput : INameOutput
    {
        public List<string> WrittenNames { get; } = new();

        public void Write(IReadOnlyList<PersonName> names) =>
            WrittenNames.AddRange(names.Select(name => name.FullName));
    }
}
