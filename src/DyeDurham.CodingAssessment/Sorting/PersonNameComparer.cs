using DyeDurham.CodingAssessment.Domain;

namespace DyeDurham.CodingAssessment.Sorting;

/// <summary>
/// Orders names by last name, then by each given name in turn.
/// When all shared given names match, the name with fewer given names comes first.
/// Case and accents are compared the natural way ("émile" sits next to "Emile");
/// names that differ only by case are then ordered by exact characters so the result never changes between runs.
/// </summary>
public sealed class PersonNameComparer : IComparer<PersonName>
{
    public int Compare(PersonName? x, PersonName? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (x is null) return -1;
        if (y is null) return 1;

        var ignoringCase = CompareUsing(StringComparer.InvariantCultureIgnoreCase, x, y);
        if (ignoringCase != 0) return ignoringCase;

        return CompareUsing(StringComparer.Ordinal, x, y);
    }

    private static int CompareUsing(StringComparer comparer, PersonName x, PersonName y)
    {
        var byLastName = comparer.Compare(x.LastName, y.LastName);
        if (byLastName != 0) return byLastName;

        var sharedGivenNameCount = Math.Min(x.GivenNames.Count, y.GivenNames.Count);
        for (var position = 0; position < sharedGivenNameCount; position++)
        {
            var byGivenName = comparer.Compare(x.GivenNames[position], y.GivenNames[position]);
            if (byGivenName != 0) return byGivenName;
        }

        return x.GivenNames.Count.CompareTo(y.GivenNames.Count);
    }
}
