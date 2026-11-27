using DyeDurham.CodingAssessment.Domain;
using DyeDurham.CodingAssessment.Input;
using DyeDurham.CodingAssessment.Output;
using DyeDurham.CodingAssessment.Parsing;
using DyeDurham.CodingAssessment.Sorting;

namespace DyeDurham.CodingAssessment.Tests;

public class NameSortingServiceTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Run_ValidNames_WritesSameSortedListToEveryOutput(int outputCount)
    {
        InMemoryNameSource source = new("Janet Parsons", "Marin Alvarez", "Adonis Julius Archer");
        InMemoryNameOutput[] outputs = CreateOutputs(outputCount);
        NameSortingService service = CreateService(source, outputs);

        service.Run();

        string[] expected = new[] { "Marin Alvarez", "Adonis Julius Archer", "Janet Parsons" };
        Assert.All(outputs, output => Assert.Equal(expected, output.WrittenNames));
    }

    [Theory]
    [InlineData("Madonna")]
    [InlineData("One Two Three Four Last")]
    public void Run_InvalidName_ThrowsAndWritesNothing(string invalidLine)
    {
        InMemoryNameSource source = new("Janet Parsons", invalidLine);
        InMemoryNameOutput[] outputs = CreateOutputs(2);
        NameSortingService service = CreateService(source, outputs);

        Assert.Throws<InvalidNameException>(() => service.Run());
        Assert.All(outputs, output => Assert.Empty(output.WrittenNames));
    }

    private static InMemoryNameOutput[] CreateOutputs(int count) =>
        Enumerable.Range(0, count).Select(_ => new InMemoryNameOutput()).ToArray();

    private static NameSortingService CreateService(INameSource source, IEnumerable<INameOutput> outputs) =>
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
