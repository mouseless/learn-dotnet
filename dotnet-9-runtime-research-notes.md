# .NET 9 Runtime Research Notes

## Attribute model for feature switches with trimming support

### FeatureSwitchDefinitionAttribute

```csharp
if (Feature.IsSupported)
    Feature.Implementation();

public class Feature
{
    [FeatureSwitchDefinition("Feature.IsSupported")]
    internal static bool IsSupported => AppContext.TryGetSwitch("Feature.IsSupported", out bool isEnabled) ? isEnabled : true;

    internal static void Implementation() => ...;
}

<ItemGroup>
  <RuntimeHostConfigurationOption Include="Feature.IsSupported" Value="false" Trim="true" />
</ItemGroup>
```

### FeatureGuardAttribute

```csharp
if (Feature.IsSupported)
    Feature.Implementation();

public class Feature
{
    [FeatureGuard(typeof(RequiresDynamicCodeAttribute))]
    internal static bool IsSupported => RuntimeFeature.IsDynamicCodeSupported;

    [RequiresDynamicCode("Feature requires dynamic code support.")]
    internal static void Implementation() => ...; // Uses dynamic code
}
```

## UnsafeAccessorAttribute supports generic parameters

## Faster exceptions

It comes enabled by default. But it can be activated in the old system.
