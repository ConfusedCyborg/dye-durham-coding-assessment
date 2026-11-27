# Name Sorter

A console app that sorts a list of names by last name, then by given names.
It prints the sorted list to the screen and writes it to `sorted-names-list.txt` in the current folder,
replacing that file if it already exists.

## Requirements

The .NET 10 SDK (10.0.103 or a later 10.0 SDK, see `global.json`).

## Run

From the repository root:

```
dotnet run --project src/DyeDurham.CodingAssessment -- ./unsorted-names-list.txt
```

Or publish the executable once, then call it the way the brief does:

```
dotnet publish src/DyeDurham.CodingAssessment -c Release -o publish
./publish/name-sorter ./unsorted-names-list.txt
```

## Test

```
dotnet test
```

The end-to-end tests run the real program in a temporary folder, so they also check the screen output,
the written file and the exit codes.

## Input

One name per line: one to three given names, then the last name as the last word.
Blank lines are skipped and extra spaces between words are ignored.

## Sorting rule

1. Last name.
2. Then each given name in order: first given name, then second, then third.
3. If all the given names they share are equal, the name with fewer given names comes first.

Upper and lower case are treated the same, and accented letters sort next to their plain letters
(invariant culture, case insensitive). Names that differ only by case are then ordered by their exact
characters, so the output is the same on every run.

## Errors

The program stops without writing anything if the input has a problem, and returns an exit code:

| Exit code | Meaning |
|-----------|---------|
| 0 | Names sorted and written. |
| 1 | Not exactly one argument given. Prints the usage. |
| 2 | The input file does not exist. |
| 3 | A line has no given name or more than three. The message gives the line number. |

## Project layout

```
src/DyeDurham.CodingAssessment/
  Program.cs                     Entry point. Checks the argument and picks the concrete classes.
  NameSortingService.cs          Reads, parses, sorts, then writes to every output.
  ExitCodes.cs                   Exit code values.
  Domain/PersonName.cs           A name: one to three given names and a last name.
  Parsing/NameParser.cs          Turns lines of text into names.
  Sorting/PersonNameComparer.cs  The sorting rule above.
  Sorting/NameSorter.cs          Sorts names with that comparer.
  IO/FileNameSource.cs           Reads the input file.
  IO/ConsoleNameOutput.cs        Prints names to the screen.
  IO/FileNameOutput.cs           Writes names to a file.
tests/DyeDurham.CodingAssessment.Tests/
  One test class per class above, plus EndToEnd/ for the real program.
```

Each class has one job and the service only knows the interfaces (`INameSource`, `INameParser`,
`INameSorter`, `INameOutput`). To send the sorted names somewhere new, add a class that implements
`INameOutput` and add it to the list in `Program.cs`. Nothing else changes.
