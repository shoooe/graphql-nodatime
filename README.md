# GraphQL.NodaTime

This library introduces NodaTime types to GraphQL.

## Installation

The library is hosted on [NuGet](https://www.nuget.org/), therefore you just need to run:

```
Install-Package GraphQL.NodaTime
```

or:

```
dotnet add package GraphQL.NodaTime
```

## Setup

You should register only the types you use by adding:

```c#
services.AddSingleton<InstantGraphType>();
services.AddSingleton<LocalDateGraphType>();
// ...
```

in your `Startup`'s `ConfigureServices` method.

Here's a list of all supported types:

| NodaTime        | GraphQL type  |
| ------------- | ------------- |
| Instant      | InstantGraphType |
| IsoDayOfWeek      | IsoDayOfWeekGraphType |
| LocalDate      | LocalDateGraphType |
| LocalTime      | LocalTimeGraphType |

## Design decisions

### Strictness

NodaTime chooses to be strict about what text patterns to allow for its types.

We decided that such a rigid policy is not well suited in the context of GraphQL.
GraphQL consumers use different tools (libraries, languages, etc.) which likely push different ways
to serialize time-related types.

For this reason, this library tries to be as lenient as possible and accept alternative patterns, 
as long as there's no ambiguity.

If there's demand for strict versions of these types, a separate namespace (e.g. `GraphQL.NodaTime.Strict`) will be used
to accomodate those.

#### A case study

For example text patterns that end with an offset (`2019-10-02T12:14:18+0130`) are not readable into an `Instant`
and should, instead, be read into an `OffsetDateTime`.

Given that there exist a conversion between an `OffsetDateTime` and an `Instant`, we chose to 
implement a more flexible parser which accepts the same set of values between the two types.

The only difference being that the offset is kept when reading into an `OffsetDateTime` and it's instead normalized
when reading into an `Instant`.