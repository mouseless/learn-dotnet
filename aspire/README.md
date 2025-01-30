# Aspire

.NET Aspire helps manage applications, services (such as Redis), and other
project dependencies. It also provides a dashboard for monitoring. Aspire
consists of two main components: `AppHost` and `ServiceDefaults`.

> [!WARNING]
>
> Only Azure supports using Aspire in cloud.

> [!NOTE]
>
> We looked into it and found it more suitable for local testing than `docker
> compose`, but not for production.

## AppHost

AppHost is the orchestration project where we manage the referenced services.
Projects can be added as follows:

[program.cs]
```csharp
var builder = DistributedApplication.CreateBuilder(args);

var projectA = builder.AddProject<Projects.ProjectA>("serviceA").WithExternalHttpEndpoints();

builder.AddProject<Projects.ProjectB>("serviceB")
    .WithExternalHttpEndpoints()
    .WithReference(projectA)
    .WaitFor(projectA);

builder.Build().Run();
```

In the example above, two service projects are added. Using extensions, we can
configure various settings and behaviors for these projects.

Once the AppHost project starts, all referenced projects will automatically
launch, eliminating the need to start them manually.

After AppHost is running, the [dashboard](#dashboard) becomes available. The
dashboard allows us to view the added projects and monitor their details
(telemetry, etc.), which are provided through the
[ServiceDefaults](#servicedefault) module.

## ServiceDefaults

ServiceDefaults provides extensions that can be used in the added projects.
These extensions collect project-related data, which AppHost utilizes to display
information on the dashboard. Extension can be used as follows:

[program.cs]
```csharp
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults(); // #1

...

var app = builder.Build();

...

app.MapDefaultEndpoints(); // #2

app.Run();
```

## Adding or Creating Aspire

To create a new Aspire project, you can use the following command:

```sh
dotnet new aspire
```

If you want to add Aspire to an existing project, modify the `.csproj` file of
a Web API project as follows to designate it as an AppHost project:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <IsAspireHost>true</IsAspireHost>
    ...
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Aspire.Hosting.AppHost" />
    ...
  </ItemGroup>

</Project>
```

The AppHost project also requires a class library project(`ServiceDefaults`
projects). Create a new class  library and update its `.csproj` file as follows:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <IsAspireSharedProject>true</IsAspireSharedProject>
  </PropertyGroup>

  <ItemGroup>
    <FrameworkReference Include="Microsoft.AspNetCore.App" />

    <!-- These dependencies are optional and can be adjusted as needed. -->
    <PackageReference Include="Microsoft.Extensions.Http.Resilience" />
    <PackageReference Include="Microsoft.Extensions.ServiceDiscovery" />
    <PackageReference Include="OpenTelemetry.Exporter.OpenTelemetryProtocol" />
    <PackageReference Include="OpenTelemetry.Extensions.Hosting" />
    <PackageReference Include="OpenTelemetry.Instrumentation.AspNetCore" />
    <PackageReference Include="OpenTelemetry.Instrumentation.Http" />
    <PackageReference Include="OpenTelemetry.Instrumentation.Runtime" />
  </ItemGroup>

</Project>
```

If you encounter the `error ASPIRE006` SDK error while adding Aspire to an
existing project, update the `.csproj` file to specify the correct SDK version:

```xml
<Sdk Name="Aspire.AppHost.Sdk" Version="9.0.0" />
```

## Dashboard

The Aspire dashboard provides a live interface for monitoring telemetry data,
viewing logs, and starting or stopping projects in real time.