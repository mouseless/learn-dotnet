using Microsoft.CodeAnalysis.Text;
using Newtonsoft.Json;

namespace CodeGen;

public static class NewtonsoftJsonExtensions
{
    public static string Serialize(this List<ServiceModel> source) => JsonConvert.SerializeObject(source);
    public static T Deserialize<T>(this SourceText source) => JsonConvert.DeserializeObject<T>(source.ToString());
}