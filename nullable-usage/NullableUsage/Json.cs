using System.Text.Json;

namespace NullableUsage;

public class Json
{
    public string SerializeWithFormat(object @object) =>
        JsonSerializer.Serialize(@object, options: new JsonSerializerOptions { WriteIndented = true });
}