namespace DyeDurham.CodingAssessment.Domain;

/// <summary>
/// A person's name: one to three given names followed by a single last name.
/// </summary>
public sealed class PersonName
{
    public const int MinGivenNames = 1;
    public const int MaxGivenNames = 3;

    public PersonName(IEnumerable<string> givenNames, string lastName)
    {
        List<string> givenNameList = givenNames.ToList();

        if (!IsValidGivenNameCount(givenNameList.Count))
        {
            throw new ArgumentException(
                $"A name must have between {MinGivenNames} and {MaxGivenNames} given names, but had {givenNameList.Count}.",
                nameof(givenNames));
        }

        GivenNames = givenNameList.AsReadOnly();
        LastName = lastName;
    }

    public IReadOnlyList<string> GivenNames { get; }

    public string LastName { get; }

    public string FullName => string.Join(' ', GivenNames.Append(LastName));

    /// <summary>True when a name may have this many given names (one to three).</summary>
    public static bool IsValidGivenNameCount(int count) =>
        count >= MinGivenNames && count <= MaxGivenNames;

    /// <summary>
    /// The given name at a position (0 is the first), or null if the name has fewer given names.
    /// Null sorts before any text, so "Adam Smith" comes before "Adam John Smith".
    /// </summary>
    public string? GivenNameAt(int position) =>
        position < GivenNames.Count ? GivenNames[position] : null;
}
