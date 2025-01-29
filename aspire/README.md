# Aspire

.Net Aspire, uygulamalarımızın, hizmetleri(redis vb) ve diğer proje bağımlılıklarını yönetmemizi sağlar. Bunları yaparken bir dashboard ile izleme imkanı sunar. Bir projeye aspire eklemek için iki projeye ihtiyacımız var. Bunlar `AppHost` ve `ServiceDefaults`'dir.

## Add or Create Aspire

Yeni bir proje oluşturulacaksa `dotnet new aspire` komutu ile hazır template kullanılabilir.

Var olan projeye eklemek isteniyorsa yeni bir WebApi'nin .csproj dosyasında aşağıdaki ayarları yapmak AppHost projesi için yeterlidir

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

AppHost projenin birde classlib projesine ihtiyacı vardır. Yeni bir classlib yaratılarak .csproj içerisinde aşağıdaki gibi düzenlemek yeterlidir.

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <IsAspireSharedProject>true</IsAspireSharedProject>
  </PropertyGroup>

  <ItemGroup>
    <FrameworkReference Include="Microsoft.AspNetCore.App" />

    <!-- Burası gerekli değildir. Hepsi Aspire'ın componentleridir. İhtiyaca göre arttırılabilir, azaltılabilir. -->
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

## Dashboard

Dashboard'ta projelerin ayakta olup olmamasını izlemek, logları görmek gb bir çok özelliği canlı bir şekilde izleyebileceğimiz bir arayüz sunuyor.