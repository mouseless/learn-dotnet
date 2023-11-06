# Publish Over Dockerfile

> :warning:
>
> Until there is a support for credentials in remote image repositories we will
> continue to use `Dockerfile`.

When dotnet publish, give `--os`, `--arch` and `/t` values to create an image in
publish.

For example

```bash
dotnet publish --os linux --arch x64 /t:PublishContainer -c Release
```

> :warning:
>
> Before you run docker compose, you need to publish image file with publish.

> :information_source:
>
> `Microsoft.NET.Build.Containers` package is needed for containerize.

We can pass parameters to the containerize process with properties like
`<ContainerPort>`, `<ContainerWorkingDirectory>`,
`<ContainerEnvironmentVariable>`, `<ContainerEntrypoint>`,
`<ContainerEntrypointArgs>`.

## Docker Compose Files

We use docker-compose to run it locally. When running, we give the
`ContainerRepository` value we give in `.csproj` to the image.

For example look [docker-compose](/docker-compose.yml)
