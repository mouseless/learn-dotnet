# Central Package Management

We use central package management to manage packages from one place and not to
go and upgrade package version one by one from `.csproj`.

## `Directory.Packages.props`

This is where we manage the packages. We set `<ManagePackageVersionsCentrally>`
to true so that projects can see the packages introduced here. With
`<PackageVersion>` we give the packages with their versions.

Look [Directory.Packages.props](Directory.Packages.props) for example

> [!NOTE]
>
> If you had multiple `Directory.Packages.props` files in your repository, the
> file that is closest to your project's directory will be evaluated for it and
> it must manually import the next one if so desired.

## In `.csproj`

All that is done here is to add the package without the version number. You can
override an individual package version by using the `VersionOverride` property
on a `<PackageReference />` item. This overrides any `<PackageVersion />`
defined centrally.
