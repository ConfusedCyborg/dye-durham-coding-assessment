namespace DyeDurham.CodingAssessment;

/// <summary>
/// The values the program returns to the shell, so scripts can tell what went wrong.
/// </summary>
public static class ExitCodes
{
    public const int Success = 0;
    public const int WrongArguments = 1;
    public const int FileNotFound = 2;
    public const int InvalidName = 3;
}
