# GraphQL.NodaTime

This library introduces GraphQL types that can be mapped to NodaTime types.

### Installation

```
Install-Package GraphQL.NodaTime
```

or

```
dotnet add package GraphQL.NodaTime
```

### Types

You can register the types you use like any other GraphQL type by adding:

```
services.AddSingleton<InstantGraphType>();
services.AddSingleton<LocalDateGraphType>();
...
```

in your `Startup`'s `ConfigureServices` method.

Here's a list of all supported types:

| NodaTime        | GraphQL type  |
| ------------- | ------------- |
| Instant      | InstantGraphType |
| IosDayOfWeek      | IsoDayOfWeekGraphType |
| LocalDate      | LocalDateGraphType |
| LocalTime      | LocalTimeGraphType |