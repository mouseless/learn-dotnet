# Analyzers

We run Analyzers before build to improve code quality, fix style issues and
ensure standardized code writing.

## Stylecop.Analyzers

We use `Stylecop.Analyzers` to analyze source code and provide consistent and
standardized coding styles.

See [Stylecop.Analyzers.csproj](/analyzers/Stylecop.Analyzers/Stylecop.Analyzers.csproj)
to see how we added it.

You can check
[ConventionallyCorrect.cs](/analyzers/Stylecop.Analyzers/ConventionallyCorrect.cs)
to see how we write code using our conventions.

### `stylecop.ruleset`

We use `stylecop.ruleset` to override Stylecop's default ruleset and change
some rules to give error instead of warning and not build. See
[stylecop.ruleset](/analyzers/Stylecop.Analyzers/stylecop.ruleset) to see how
we override the ruleset.

See [StyleCopAnalyzers/documentation](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/tree/master/documentation)
to see all the rules.
