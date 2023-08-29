# Analyzers

We run Analyzers before build to improve code quality, fix style issues and
ensure standardized code writing.

## Stylecop.Analyzers

We use `Stylecop.Analyzers` to analyze source code and provide consistent and
standardized coding styles.

See [Stylecop.Analyzers.csproj](/analyzers/Stylecop.Analyzers/Stylecop.Analyzers.csproj)
to see how we added `Stylecop.Analyzers` to our project.

You can check
[ConventionallyCorrect.cs](/analyzers/Stylecop.Analyzers/ConventionallyCorrect.cs)
to see how we write code using our coding style standards.

### `stylecop.ruleset`

We use `stylecop.ruleset` to override Stylecop's default ruleset. We enabled
the rules that are compatible with our coding style standards and disabled the
rest. Changed severity of the enabled rules to error, so that we get an error
from github actions build if the commit contains non standard code. See
[stylecop.ruleset](/analyzers/Stylecop.Analyzers/stylecop.ruleset) to see how
we override the ruleset.

> :warning:
>
> `stylecop.ruleset` must be included in `.csproj`.

`Stylecop.Analyzers.csproj`
```xml
...
  <PropertyGroup>
    <CodeAnalysisRuleSet>stylecop.ruleset</CodeAnalysisRuleSet>
  </PropertyGroup>
...
```

See [StyleCopAnalyzers/documentation](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/tree/master/documentation)
to see all the rules.
