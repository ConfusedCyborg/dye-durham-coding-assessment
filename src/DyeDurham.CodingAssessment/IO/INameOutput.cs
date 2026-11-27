using DyeDurham.CodingAssessment.Domain;

namespace DyeDurham.CodingAssessment.IO;

/// <summary>
/// Somewhere the sorted names are written to. Add a new implementation to send them somewhere new.
/// </summary>
public interface INameOutput
{
    void Write(IReadOnlyList<PersonName> names);
}
