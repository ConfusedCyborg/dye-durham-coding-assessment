# Name Sorter

[![Build and test](https://github.com/ConfusedCyborg/dye-durham-coding-assessment/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/ConfusedCyborg/dye-durham-coding-assessment/actions/workflows/build.yml)

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

In the Windows command prompt (cmd.exe), the second line is `publish\name-sorter ./unsorted-names-list.txt`.

## Test

```
dotnet test
```

The end-to-end tests run the real program in a temporary folder, so they also check the screen output,
the written file and the exit codes. Each feature is tested both with valid input and with the input
that should fail, mostly as xUnit theories so one test covers several cases.

To measure coverage (100% of lines and branches at the time of writing):

```
dotnet test --collect:"XPlat Code Coverage"
```

## Input

One name per line: one to three given names, then the last name as the last word.
Blank lines are skipped and extra whitespace between words (spaces, tabs, non-breaking spaces) is ignored.

## Sorting rule

1. Last name.
2. Then each given name in order: first given name, then second, then third.
3. If all the given names they share are equal, the name with fewer given names comes first.

Upper and lower case are treated the same, and accented letters sort next to their plain letters
(invariant culture, case insensitive). Names that differ only by case are then ordered by their exact
characters (upper case first), so the result does not depend on the order of the input file.

## Errors

If the input has a problem, the program stops before writing anything. The exit code says what happened:

| Exit code | Meaning |
|-----------|---------|
| 0 | Names sorted and written. |
| 1 | Not exactly one argument given. Prints the usage. |
| 2 | The input file does not exist. |
| 3 | One or more lines have no given name or more than three. The message lists every one with its line number. |
| 4 | A file could not be read or written, for example `sorted-names-list.txt` is read-only or open in another program. |

## Project layout

```
src/DyeDurham.CodingAssessment/
  Program.cs                   Entry point. Checks the argument and picks the concrete classes.
  NameSortingService.cs        Reads, parses, sorts, then writes to every output.
  ExitCodes.cs                 Exit code values.
  Domain/PersonName.cs         A name: one to three given names and a last name.
  Parsing/NameParser.cs        Turns lines of text into names.
  Sorting/NameSorter.cs        The sorting rule above, as one LINQ OrderBy/ThenBy chain.
  Input/FileNameSource.cs      Reads the input file.
  Output/ConsoleNameOutput.cs  Prints names to the screen.
  Output/FileNameOutput.cs     Writes names to a file.
tests/DyeDurham.CodingAssessment.Tests/
  One test class per class above, plus EndToEnd/ for the real program.
```

Each class has one job and the service only knows the interfaces (`INameSource`, `INameParser`,
`INameSorter`, `INameOutput`). To send the sorted names somewhere new, add a class that implements
`INameOutput` and add it to the list in `Program.cs`. Nothing else changes.

Code style lives in `.editorconfig` and is checked on every build: types are always written out (no `var`),
and `new()` is used when the type is already on the left.
