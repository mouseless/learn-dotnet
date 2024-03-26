# Analyzers

We run Analyzers before build to improve code quality, fix style issues and
ensure standardized code writing.

We use the `.editorconfig` file to manage analyzers. See
[.editorconfig](/.editorconfig) to see how we manage it. The following
explanations will go through `.editorconfig`.

## Code Style Rules

We use Code Style rules from Microsoft's Code Analysis tool with dotnet to
maintain our code writing standard and keep code quality high.

We use the rules we will use in `error` mode.

```editorconfig
dotnet_diagnostic.IDE{rule number}.severity = error
```

Instead of setting rules we don't use to `none`, we close the category. We can
then override it by opening the rules we use under it.

```editorconfig
dotnet_analyzer_diagnostic.category-Style.severity = none
dotnet_diagnostic.IDE{rule number}.severity = error
```

## Stylecop.Analyzers

We use `Stylecop.Analyzers` for situations where the Code Analysis tool is
insufficient.

Since `Stylecop.Analyzers` has a lot of rules and we don't use most of them, we
prefer to keep the direct categories closed.

```editorconfig
dotnet_analyzer_diagnostic.category-StyleCop.CSharp.{category}.severity = none
```

To activate any rule

```editorconfig
dotnet_diagnostic.SA{rule number}.severity = error
```

See [Stylecop.Analyzers.csproj](/analyzers/Stylecop.Analyzers/Stylecop.Analyzers.csproj)
to see how we added `Stylecop.Analyzers` to our project.

You can check
[ConventionallyCorrect.cs](/analyzers/Stylecop.Analyzers/ConventionallyCorrect.cs)
to see how we write code using our coding style standards.

See [StyleCopAnalyzers/documentation](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/tree/master/documentation)
to see all the rules.
