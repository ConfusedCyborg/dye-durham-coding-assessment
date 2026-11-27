using DyeDurham.CodingAssessment.Domain;

namespace DyeDurham.CodingAssessment.Tests;

/// <summary>
/// Builds names for tests from full names such as "Adonis Julius Archer", so test data stays easy to read.
/// </summary>
public static class TestNames
{
    public static List<PersonName> From(params string[] fullNames) =>
        fullNames
            .Select(fullName => fullName.Split(' '))
            .Select(words => new PersonName(words.SkipLast(1), words.Last()))
            .ToList();
}
