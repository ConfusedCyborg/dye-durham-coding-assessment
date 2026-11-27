using DyeDurham.CodingAssessment.Output;

namespace DyeDurham.CodingAssessment.Tests.Output;

public class ConsoleNameOutputTests
{
    [Theory]
    [MemberData(nameof(TestNames.SampleLists), MemberType = typeof(TestNames))]
    public void Write_Names_PrintsOneFullNamePerLine(string[] fullNames)
    {
        StringWriter screen = new();
        ConsoleNameOutput output = new(screen);

        output.Write(TestNames.From(fullNames));

        string expected = string.Concat(fullNames.Select(fullName => fullName + Environment.NewLine));
        Assert.Equal(expected, screen.ToString());
    }
}
