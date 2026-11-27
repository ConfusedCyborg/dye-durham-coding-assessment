using DyeDurham.CodingAssessment.Domain;
using DyeDurham.CodingAssessment.IO;
using DyeDurham.CodingAssessment.Parsing;
using DyeDurham.CodingAssessment.Sorting;

namespace DyeDurham.CodingAssessment;

/// <summary>
/// The whole job in one place: read the names, parse them, sort them, then write them to every output.
/// It only knows the interfaces, so each step can be swapped or tested on its own.
/// </summary>
public sealed class NameSortingService
{
    private readonly INameSource _source;
    private readonly INameParser _parser;
    private readonly INameSorter _sorter;
    private readonly IReadOnlyList<INameOutput> _outputs;

    public NameSortingService(INameSource source, INameParser parser, INameSorter sorter, IEnumerable<INameOutput> outputs)
    {
        _source = source;
        _parser = parser;
        _sorter = sorter;
        _outputs = outputs.ToList();
    }

    /// <exception cref="InvalidNameException">A line in the source is not a valid name. Nothing is written.</exception>
    public void Run()
    {
        IReadOnlyList<PersonName> names = _parser.Parse(_source.ReadLines());
        IReadOnlyList<PersonName> sortedNames = _sorter.Sort(names);

        foreach (INameOutput output in _outputs)
        {
            output.Write(sortedNames);
        }
    }
}
