# Localization

We use Localization to return the equivalents of string values, for example
exception messages, in the region and language of the user's choice.

## Setup

We register the localization with `.AddLocalization` and use it with
`.UseRequestLocalization`.

## Configure

We provide support for cultures like `en-US`, `tr-TR` which we often use in
configurations and we use culture providers by default.

> [!WARNING]
>
> For localization to work correctly in api requests, `SupportedUICultures` must
> be given next to `SupportedCultures`.

See [Program.cs](/localization/Localization/Program.cs) to see sample
configurations and how to add a custom provider.

## Usage

We use `IStringLocalizerFactory` to inject localization with 2 different
methods. Since we use similar resource files with the project name, we need to
use `IStringLocalizerFactory.Create()`.

See [Using Localization Factory](/localization/Localization/ArticleManager.cs)
for example usage.

## Culture Providers

We are not using custom provider for now, but if needed, we can write our own
provider and make region selection according to the data we want via
`HttpContext`.

See [Custom Provider](/localization/Localization/CustomCultureProvider.cs) for
a sample custom provider.

## Resource Files

We use `.restext` files as resource files for ease of writing and updating.

Such files need to be recognized by the system in order to be used, so they
should be added as `EmbeddedResource` from `.csproj`. See
[Localization.csproj](/localization/Localization/Localization.csproj) here for
an example.
