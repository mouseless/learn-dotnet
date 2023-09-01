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
the rules that are compatible with our coding style standards and disabled
all the other rules. Changed severity of the enabled rules to error, by doing so
we get an error from our github actions, if a code that does not follow our
standard coding style is commited. See
[stylecop.ruleset](/analyzers/Stylecop.Analyzers/stylecop.ruleset) to see how
we override the ruleset.

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

##### Spacing Rules

| Name | Code |
| :---: | :---: |
| KeywordsMustBeSpacedCorrectly | SA1000 |
| CommasMustBeSpacedCorrectly | SA1001 |
| SemicolonsMustBeSpacedCorrectly | SA1002 |
| SymbolsMustBeSpacedCorrectly | SA1003 |
| OpeningParenthesisMustBeSpacedCorrectly | SA1008 |
| OpeningSquareBracketsMustBeSpacedCorrectly | SA1010 |
| ClosingSquareBracketsMustBeSpacedCorrectly | SA1011 |
| OpeningBracesMustBeSpacedCorrectly | SA1012 |
| ClosingBracesMustBeSpacedCorrectly | SA1013 |
| OpeningGenericBracketsMustBeSpacedCorrectly | SA1014 |
| ClosingGenericBracketsMustBeSpacedCorrectly | SA1015 |
| NullableTypeSymbolsMustNotBePrecededBySpace | SA1018 |
| MemberAccessSymbolsMustBeSpacedCorrectly | SA1019 |
| IncrementDecrementSymbolsMustBeSpacedCorrectly | SA1020 |
| ColonsMustBeSpacedCorrectly | SA1024 |
| CodeMustNotContainMultipleWhitespaceInARow | SA1025 |
| CodeMustNotContainSpaceAfterNewKeywordInImplicitlyTypedArrayAllocation | SA1026 |
| UseTabsCorrectly | SA1027 |
| CodeMustNotContainTrailingWhitespace | SA1028 |

##### Readability Rules

| Name | Code |
| :---: | :---: |
| CodeMustNotContainEmptyStatements | SA1106 |
| CodeMustNotContainMultipleStatementsOnOneLine | SA1107 |
| ClosingParenthesisMustBeOnLineOfOpeningParenthesis | SA1112 |
| CommaMustBeOnSameLineAsPreviousParameter | SA1113 |
| ParameterMustFollowComma | SA1115 |
| SplitParametersMustStartOnLineAfterDeclaration | SA1116 |
| ParametersMustBeOnSameLineOrSeparateLines | SA1117 |
| UseBuiltInTypeAlias | SA1121 |
| UseStringEmptyForEmptyStrings | SA1122 |
| UseShorthandForNullableTypes | SA1125 |
| GenericTypeConstraintsMustBeOnOwnLine | SA1127 |
| ConstructorInitializerMustBeOnOwnLine | SA1128 |
| UseLambdaSyntax | SA1130 |
| UseReadableConditions | SA1131 |
| DoNotCombineFields | SA1132 |
| DoNotCombineAttributes | SA1133 |
| AttributesMustNotShareLine | SA1134 |
| EnumValuesShouldBeOnSeparateLines | SA1136 |
| ElementsShouldHaveTheSameIndentation | SA1137 |

##### Ordering Rules

| Name | Code |
| :---: | :---: |
| UsingDirectivesMustBePlacedCorrectly | SA1200 |
| ElementsMustAppearInTheCorrectOrder | SA1201 |
| StaticElementsMustAppearBeforeInstanceElements | SA1204 |
| UsingDirectivesMustBeOrderedAlphabeticallyByNamespace | SA1210 |
| UsingAliasDirectivesMustBeOrderedAlphabeticallyByAliasName | SA1211 |
| PropertyAccessorsMustFollowOrder | SA1212  |

##### Naming Rules

| Name | Code |
| :---: | :---: |
| ElementMustBeginWithUpperCaseLetter | SA1300 |
| FieldNamesMustBeginWithLowerCaseLetter | SA1306 |
| AccessibleFieldsMustBeginWithUpperCaseLetter | SA1307 |
| VariableNamesMustNotBePrefixed | SA1308 |
| StaticReadonlyFieldsMustBeginWithUpperCaseLetter | SA1311 |
| VariableNamesMustBeginWithLowerCaseLetter | SA1312 |
| ParameterNamesMustBeginWithLowerCaseLetter | SA1313 |
| TypeParameterNamesMustBeginWithT | SA1314 |

##### Maintainability Rules

| Name | Code |
| :---: | :---: |
| FileMayOnlyContainASingleNamespace | SA1403 |

##### Layout Rules

| Name | Code |
| :---: | :---: |
| OpeningBracesMustNotBeFollowedByBlankLine | SA1505 |
| ClosingBracesMustNotBePrecededByBlankLine | SA1508 |
| OpeningBracesMustNotBePrecededByBlankLine | SA1509 |
| ClosingBraceMustBeFollowedByBlankLine | SA1513 |

##### Documentation Rules

| Name | Code |
| :---: | :---: |
| FileNameMustMatchTypeName | SA1649 |

##### Alternative Rules

| Name | Code |
| :---: | :---: |
| FieldNamesMustBeginWithUnderscore | SX1309 |
