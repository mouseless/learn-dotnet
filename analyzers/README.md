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

### `.editorconfig`

We use `.editorconfig` to override Stylecop's default rule. We enabled the rules
that are compatible with our coding style standards and disabled all the other
rules. Changed severity of the enabled rules to error, by doing so we get an
error from our github actions, if a code that does not follow our standard
coding style is commited. See [.editorconfig](/.editorconfig) to see  how we
override the rule.

### `stylecop.ruleset`

Another option is to use `stylecop.ruleset` to override Stylecop's default
ruleset.

> :warning:
>
> `stylecop.ruleset` must be included in `.csproj`.

`Stylecop.Analyzers.csproj`
```xml
<PropertyGroup>
  <CodeAnalysisRuleSet>stylecop.ruleset</CodeAnalysisRuleSet>
</PropertyGroup>
```

See [StyleCopAnalyzers/documentation](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/tree/master/documentation)
to see all the rules.

#### Included Rules

##### Readability Rules

| Name | Code |
| --- | --- |
| OpeningParenthesisMustBeOnDeclarationLine | SA1110 |
| ClosingParenthesisMustBeOnLineOfOpeningParenthesis | SA1112 |
| CommaMustBeOnSameLineAsPreviousParameter | SA1113 |
| UseStringEmptyForEmptyStrings | SA1122 |
| UseReadableConditions | SA1131 |
| DoNotCombineFields | SA1132 |
| DoNotCombineAttributes | SA1133 |
| EnumValuesShouldBeOnSeparateLines | SA1136 |

##### Ordering Rules

| Name | Code |
| --- | --- |
| UsingDirectivesMustBeOrderedAlphabeticallyByNamespace | SA1210 |
| UsingAliasDirectivesMustBeOrderedAlphabeticallyByAliasName | SA1211 |
| PropertyAccessorsMustFollowOrder | SA1212  |

##### Naming Rules

| Name | Code |
| --- | --- |
| FieldNamesMustBeginWithLowerCaseLetter | SA1306 |
| VariableNamesMustNotBePrefixed | SA1308 |
| StaticReadonlyFieldsMustBeginWithUpperCaseLetter | SA1311 |
| TypeParameterNamesMustBeginWithT | SA1314 |

##### Layout Rules

| Name | Code |
| --- | --- |
| CodeMustNotContainMultipleBlankLinesInARow | SA1507 |
| ClosingBraceMustBeFollowedByBlankLine | SA1513 |

##### Documentation Rules

| Name | Code |
| --- | --- |
| FileNameMustMatchTypeName | SA1649 |
