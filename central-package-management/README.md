# Central Package Management

We use central package management to manage packages from one place and not to
go and upgrade package version one by one from `.csproj`.

## `Directory.Packages.props`

This is where we manage the packages. We set `<ManagePackageVersionsCentrally>`
to true so that projects can see the packages introduced here. With
`<PackageVersion>` we give the packages with their versions.

Look [Directory.Packages.props](Directory.Packages.props) for example

## In `.csproj`

All that is done here is to add the package without the version number.
