# ListComparerPrototype

**C# list comparison — training prototype for WFX and SAG**

## Purpose

This repository is a prototype for comparing the contents of two lists. It is used as shared training material for colleagues from **WFX** and **SAG** to explore comparison requirements, C# implementation techniques, and ways to validate results together.

> **Prototype status:** This project is intended for learning and discussion, not as a production-ready comparison or synchronization engine.

## Learning objectives

After working through this prototype, participants should be able to:

- Explain what it means for two list entries to be equal.
- Identify entries present only in the first list, only in the second list, or in both.
- Distinguish comparison by value from comparison by a business key.
- Discuss the impact of duplicates, null values, order, and case sensitivity.
- Write tests for typical and edge-case comparisons.
- Assess which comparison strategy fits a WFX–SAG integration scenario.

## Getting started

### Prerequisites

- Git
- A compatible .NET SDK (check the target framework in the project's `.csproj` file)
- Visual Studio, Visual Studio Code with C# support, or another C# IDE

### Clone the repository

```bash
git clone https://github.com/mapfel/ListComparerPrototype.git
cd ListComparerPrototype
```

### Find and run the project

The application source is under [`src/`](src/). Find the solution file, then use the appropriate command:

```bash
# If a solution is available
dotnet build path/to/solution.sln

# Or run an executable project directly
dotnet run --project path/to/project.csproj
```

Replace the placeholder paths with actual paths in the repository. If the project targets an older framework or uses dependencies that require additional setup, follow the project configuration in `src/`.

## Comparison concepts

Assume two sets of identifiers:

```csharp
var source = new List<int> { 1, 2, 3, 4 };
var target = new List<int> { 3, 4, 5 };

var onlyInSource = source.Except(target).ToList(); // 1, 2
var onlyInTarget = target.Except(source).ToList(); // 5
var inBoth = source.Intersect(target).ToList();   // 3, 4
```

This is a **standalone training example**; it does not claim to reproduce the repository's exact implementation.

| Result | Meaning | Example |
| --- | --- | --- |
| Only in source | Present in the first list, absent from the second | `1, 2` |
| Only in target | Present in the second list, absent from the first | `5` |
| In both | Present in both lists | `3, 4` |

### Comparing objects

For object lists, first agree on **identity** and **equality**. Two records may have the same identifier but different values. For example:

```csharp
record Item(int Id, string Name);

var source = new List<Item> { new(1, "Alpha"), new(2, "Beta") };
var target = new List<Item> { new(1, "Alpha updated"), new(3, "Gamma") };

var sourceById = source.ToDictionary(x => x.Id);
var targetById = target.ToDictionary(x => x.Id);

var addedIds = targetById.Keys.Except(sourceById.Keys);
var removedIds = sourceById.Keys.Except(targetById.Keys);
var changedIds = sourceById.Keys.Intersect(targetById.Keys)
    .Where(id => sourceById[id].Name != targetById[id].Name);
```

Here, `Id` is the business key and `Name` is a value that may change. `ToDictionary` throws if duplicate IDs exist, so a real implementation must define a duplicate-handling policy.

## Workshop: WFX and SAG

### Exercise 1 — Explore the code

Find the comparison entry point in `src/`. Identify the input types, output format, and comparison rules. Run the existing implementation and record the observed result.

### Exercise 2 — Verify basic scenarios

Test equal lists, disjoint lists, an empty source list, an empty target list, and lists with shared elements. Agree on expected results before running the code.

### Exercise 3 — Discuss equality

Determine whether comparison should use complete object equality or a stable identifier. Consider case sensitivity, whitespace, nulls, and different ordering.

### Exercise 4 — Handle duplicates

Add repeated entries to both lists. Decide whether duplicates should be ignored, counted, reported, or rejected. Note that LINQ `Except` and `Intersect` use set semantics.

### Exercise 5 — Detect updates

Use records with stable IDs and changing attributes. Categorize entries as **added**, **removed**, **unchanged**, or **modified**.

### Exercise 6 — Review performance and tests

Try larger lists and compare the readability and performance of nested loops, LINQ set operators, and dictionary-based lookups. Add automated tests for edge cases.

## Joint discussion questions

1. What is the authoritative source of data in a WFX–SAG exchange?
2. Which attributes identify the same logical record across systems?
3. Does ordering matter, and how should duplicates be handled?
4. Which differences require synchronization, reporting, or manual review?
5. How should invalid, missing, or conflicting records be reported?
6. What tests and acceptance criteria would be required before production use?

## Suggested acceptance criteria

- Expected outcomes are documented for all training scenarios.
- Comparison behavior is deterministic and covered by tests.
- Equality, duplicates, and null handling are explicitly defined.
- Outputs distinguish additions, removals, and modifications when applicable.
- Limitations and assumptions are documented before any production adoption.

## Scope and limitations

This README documents the repository's stated purpose and provides educational examples. It does **not** assert that the current code implements every example, exercise, or result category shown above. Participants should inspect the source in [`src/`](src/) to establish actual behavior and supported .NET versions.

## Resources

- [Repository](https://github.com/mapfel/ListComparerPrototype)
- [Microsoft Learn — LINQ](https://learn.microsoft.com/en-us/dotnet/csharp/linq/)
- [Microsoft Learn — Enumerable.Except](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.except)
- [Microsoft Learn — Enumerable.Intersect](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.intersect)

---

**Audience:** WFX and SAG colleagues · **Format:** Guided prototype walkthrough and hands-on exercises
