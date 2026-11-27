using DyeDurham.CodingAssessment.Domain;
using DyeDurham.CodingAssessment.Input;
using DyeDurham.CodingAssessment.Output;
using DyeDurham.CodingAssessment.Parsing;
using DyeDurham.CodingAssessment.Sorting;

namespace DyeDurham.CodingAssessment;

/// <summary>
/// The whole job in one place: read the names, parse them, sort them, then write them to every output.
/// It only knows the interfaces, so each step can be swapped or tested on its own.
/// Outputs are written in the order given; if one throws, the ones after it are not written.
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

    /// <exception cref="InvalidNameException">One or more lines in the source are not valid names. Nothing is written.</exception>
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
