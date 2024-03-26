# Analyzers

We run Analyzers before build to improve code quality, fix style issues and
ensure standardized code writing.

In order not to skip warnings and to have a clean typing, we raise the warnings
to error by making `<CodeAnalysisTreatWarningsAsErrors>` `true`. See
[AnalyzersSample.csproj](AnalyzersSample.csproj).

We use the `.editorconfig` file to manage analyzers. See
[.editorconfig](/.editorconfig) to see how we manage it. The following
explanations will go through `.editorconfig`.

## Code Style Rules

We use Code Style rules from Microsoft's Code Analysis tool with dotnet to
maintain our code writing standard and keep code quality high.

By setting `<AnalysisModeStyle>` to `All` and `<EnforceCodeStyleInBuild>` to
`true`, we enable Code Style suggestions/warnings and make them visible during
build.

We set the rules we will use in `error` mode.

```ini
dotnet_diagnostic.IDE<rule number>.severity = error
```

Instead of setting rules we don't use to `none`, we close the category. We can
then override it by opening the rules we use under it.

```ini
dotnet_analyzer_diagnostic.category-<category>.severity = none
dotnet_diagnostic.IDE<rule number>.severity = error
```

Since we don't use documentation in code, we make `<GenerateDocumentationFile>`
`false`. See [AnalyzersSample.csproj](AnalyzersSample.csproj).

## Stylecop.Analyzers

We use `Stylecop.Analyzers` for situations where the Code Analysis tool is
insufficient.

Since `Stylecop.Analyzers` has a lot of rules and we don't use most of them, we
prefer to keep the direct categories closed.

```ini
dotnet_analyzer_diagnostic.category-StyleCop.CSharp.<category>.severity = none
```

To activate any rule

```ini
dotnet_diagnostic.SA<rule number>.severity = error
```

See [Stylecop.Analyzers.csproj](/analyzers/Stylecop.Analyzers/Stylecop.Analyzers.csproj)
to see how we added `Stylecop.Analyzers` to our project.

You can check
[ConventionallyCorrect.cs](/analyzers/Stylecop.Analyzers/ConventionallyCorrect.cs)
to see how we write code using our coding style standards.

See [StyleCopAnalyzers/documentation](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/tree/master/documentation)
to see all the rules.
