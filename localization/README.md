# Localization

We use Localization to return the equivalents of string values, for example
exception messages, in the region and language of the user's choice.

## Setup

We register the localization with `.AddLocalization` and use it with
`.UseRequestLocalization`.

## Configure

We provide support for cultures like `en-US`, `en-TR` which we often use in
configurations and we use culture providers by default.

> [!WARNING]
>
> For localization to work correctly in api requests, `SupportedUICultures` must
> be given next to `SupportedCultures`.

See [program.cs](/localization/Localization/Program.cs) to see sample
configurations and how to add a custom provider.

## Usage

We can use `IStringLocalizer` or `IStringLocalizerFactory` to inject
localization with 2 different methods. Since we use similar resource files with
the project name, we need to use `IStringLocalizerFactory.Create()`.

See [Using Localization Factory](/localization/Localization/ArticleManager.cs)
for example usage.

## Culture Providers

We are not using custom provider for now, but if needed, we can write our own
provider and make region selection according to the data we want via
`HttpContext`.

See [Custom Provider](/localization/Localization/CustomCultureProvider.cs) for
a sample custom provider.

## Resource Files

Text translations are provided via `.resx` files. The file naming convention is:

```txt
<FullTypeName>.resx         → Default language
<FullTypeName>.<Culture>.resx → Specific cultures
```

> [!NOTE]
>
> Instead of `.resx`, a database can also be used as a source.

### Parameterized Usage

You can use placeholders like `{0}`, `{1}` in `.resx` files:

```xml
<data name="platformName" xml:space="preserve">
  <value>Streaming Platform {0:P}</value>
</data>
```
