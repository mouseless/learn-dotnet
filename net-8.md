# What's new in `.Net 8`

## Serialization

Genel yenilikler `System.Text.Json` ile ilgili olmuş. `System.Text.Json` a mı
geçsek ?

- `snake_case`, new naming policies

  ```csharp
  var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };
  JsonSerializer.Serialize(new { PropertyName = "value" }, options); // { "property_name" : "value" }
  ```

## Time abstraction

The new `TimeProvider` class and `ITimer` interface add time abstraction
functionality, which allows you to mock time in test scenarios. In addition,
you can use the time abstraction to mock Task operations that rely on time
progression using `Task.Delay` and `Task`.`WaitAsync`.

```csharp
// Get system time.
DateTimeOffset utcNow = TimeProvider.System.GetUtcNow();
DateTimeOffset localNow = TimeProvider.System.GetLocalNow();

// Create a time provider that works with a
// time zone that's different than the local time zone.
private class ZonedTimeProvider : TimeProvider
{
    private TimeZoneInfo _zoneInfo;

    public ZonedTimeProvider(TimeZoneInfo zoneInfo) : base()
    {
        _zoneInfo = zoneInfo ?? TimeZoneInfo.Local;
    }

    public override TimeZoneInfo LocalTimeZone => _zoneInfo;

    public static TimeProvider FromLocalTimeZone(TimeZoneInfo zoneInfo) =>
        new ZonedTimeProvider(zoneInfo);
}

// Create a timer using a time provider.
ITimer timer = timeProvider.CreateTimer(callBack, state, delay, Timeout.InfiniteTimeSpan);

// Measure a period using the system time provider.
long providerTimestamp1 = TimeProvider.System.GetTimestamp();
long providerTimestamp2 = TimeProvider.System.GetTimestamp();

var period = GetElapsedTime(providerTimestamp1, providerTimestamp2);
```

## UTF8 improvements

New interface `IUtf8SpanFormattable`. It has full support for all formats
(including the new "B" binary specifier) and all cultures. This means you can
now format directly to UTF8 from Byte, Complex, Char, DateOnly, DateTime,
DateTimeOffset, Decimal, Double, Guid, Half, IPAddress, IPNetwork, Int16,
Int32, Int64, Int128, IntPtr, NFloat, SByte, Single, Rune, TimeOnly, TimeSpan,
UInt16, UInt32, UInt64, UInt128, UIntPtr, and Version.

New `Utf8.TryWrite` methods provide a `UTF8-based` counterpart to the existing
`MemoryExtensions.TryWrite` methods, which are `UTF16-based`.

```csharp
static bool FormatHexVersion(
    short major,
    short minor,
    short build,
    short revision,
    Span<byte> utf8Bytes,
    out int bytesWritten) =>
    Utf8.TryWrite(
        utf8Bytes,
        CultureInfo.InvariantCulture,
        $"{major:X4}.{minor:X4}.{build:X4}.{revision:X4}",
        out bytesWritten);
```

## Methods for working with randomness

The `System.Random` and `System.Security.Cryptography.RandomNumberGenerator`
types introduce two new methods for working with randomness.

`GetItems<T>()`

```csharp
private static ReadOnlySpan<Button> s_allButtons = new[]
{
    Button.Red,
    Button.Green,
    Button.Blue,
    Button.Yellow,
};

// ...

Button[] thisRound = Random.Shared.GetItems(s_allButtons, 31);
// Rest of game goes here ...
```

`Shuffle<T>()`

```csharp
YourType[] trainingData = LoadTrainingData();
Random.Shared.Shuffle(trainingData);

IDataView sourceData = mlContext.Data.LoadFromEnumerable(trainingData);

DataOperationsCatalog.TrainTestData split = mlContext.Data.TrainTestSplit(sourceData);
model = chain.Fit(split.TrainSet);

IDataView predictions = model.Transform(split.TestSet);
// ...
```

## Performance-focused types

Collection types new `FrozenDictionary<TKey,TValue>` and `FrozenSet<T>`. These
types don't allow any changes to keys and values once a collection created.

```csharp
private static readonly FrozenDictionary<string, bool> s_configurationData =
    LoadConfigurationData().ToFrozenDictionary(optimizeForReads: true);

// ...
if (s_configurationData.TryGetValue(key, out bool setting) && setting)
{
    Process();
}
```

The new `System.Buffers.SearchValues<T>` type is designed to be passed to
methods that look for the first occurrence of any value in the passed
collection. When you create an instance of `System.Buffers.SearchValues<T>`,
all the data that's necessary to optimize subsequent searches is derived at
that time, meaning the work is done up front.

The new `System.Text.CompositeFormat` type is useful for optimizing format
strings that aren't known at compile time (for example, if the format string
is loaded from a resource file). A little extra time is spent up front to do
work such as parsing the string, but it saves the work from being done on
each use.

```csharp
private static readonly CompositeFormat s_rangeMessage =
    CompositeFormat.Parse(LoadRangeMessageResource());

// ...
static string GetMessage(int min, int max) =>
    string.Format(CultureInfo.InvariantCulture, s_rangeMessage, min, max);
```

New `System.IO.Hashing.XxHash3` and `System.IO.Hashing.XxHash128` types provide
implementations of the fast `XXH3` and `XXH128` hash algorithms.

## System.Numerics and System.Runtime.Intrinsics

Pek bizle alakası yok gibi. `Matrix`, `Vector` vb üzerinde yenilikler var.

## Data validation

New properties were added to the `RangeAttribute` and `RequiredAttribute` types

- `RangeAttribute.MinimumIsExclusive`
- `RangeAttribute.MaximumIsExclusive`

New data validation attributes added

- `System.ComponentModel.DataAnnotations.LengthAttribute`
- `System.ComponentModel.DataAnnotations.Base64StringAttribute`
- `System.ComponentModel.DataAnnotations.AllowedValuesAttribute`
- `System.ComponentModel.DataAnnotations.DeniedValuesAttribute`

## Metrics

New APIs let you attach key-value pair tags to Meter and Instrument objects
when you create them. Aggregators of published metric measurements can use the
tags to differentiate the aggregated values.

```csharp
MeterOptions options = new MeterOptions("name")
{
    Version = "version",
    // Attach these tags to the created meter
    Tags = new TagList() {
        { "MeterKey1", "MeterValue1" },
        { "MeterKey2", "MeterValue2" }
    }
};

Meter meter = meterFactory.Create(options);

Instrument instrument = meter.CreateCounter<int>(
    "counter",
    null,
    null,
    new TagList() { { "counterKey1", "counterValue1" } }
);
instrument.Add(1);
```

İşimize yarar mı bilemedim.

## Cryptography

Now support for the `SHA-3` hashing primitives.

## Networking

Until now, the proxy types that HttpClient supported all allowed a
"man-in-the-middle" to see which site the client is connecting to, even for
`HTTPS URIs`. `HttpClient` now supports `HTTPS` proxy, which creates an
encrypted channel between the client and the proxy so all requests can be
handled with full privacy.

To enable `HTTPS` proxy, set the all_proxy environment variable, or use the
`WebProxy` class to control the proxy programmatically.

Unix: `export all_proxy=https://x.x.x.x:3218`
Windows: `set all_proxy=https://x.x.x.x:3218`

## Stream-based ZipFile methods

.NET 8 includes new overloads of

- `ZipFile.CreateFromDirectory` that allow you to collect all the files
  included in a directory and zip them
- `ZipFile.ExtractToDirectory` overloads let you provide a stream containing
  a zipped file and extract its contents into the filesystem

## Extension libraries

### Keyed DI services

### Options validation

### LoggerMessageAttribute constructors

### Extensions metrics

### Hosted lifecycle services

### System.Numerics.Tensors.TensorPrimitives
