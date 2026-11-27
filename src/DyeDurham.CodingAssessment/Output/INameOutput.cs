using DyeDurham.CodingAssessment.Domain;

namespace DyeDurham.CodingAssessment.Output;

/// <summary>
/// Somewhere the sorted names are written to. Add a new implementation to send them somewhere new.
/// </summary>
public interface INameOutput
{
    /// <summary>Writes the names in the order given, one full name per line.</summary>
    void Write(IReadOnlyList<PersonName> names);
}
