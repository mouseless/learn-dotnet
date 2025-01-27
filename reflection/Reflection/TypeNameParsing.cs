using Microsoft.Extensions.Logging;
using System.Reflection.Metadata;

namespace LearnReflection;

public class TypeNameParsing(ILogger<TypeNameParsing> _logger)
{
    Dictionary<string, Type> _allowList =
        new(){
            {typeof(string).Name, typeof(string)},
            {typeof(int).Name, typeof(int)},
            {typeof(double).Name, typeof(double)}
        };

    public bool IsThisTypeOk(Type type)
    {
        if (!TypeName.TryParse(type.FullName, out TypeName? parsed))
        {
            throw new ArgumentException("Type name parsing failed");
        }

        if (!_allowList.ContainsKey(parsed.Name))
        {
            _logger.LogInformation($"Type {parsed.Name} is not allowed");

            return false;
        }

        _logger.LogInformation(@$"Type {parsed.Name} is allowed. Type details:
        - Name: {parsed.Name}
        - FullName: {parsed.FullName}
        - Is Generic: {parsed.IsConstructedGenericType}");

        return true;
    }
}