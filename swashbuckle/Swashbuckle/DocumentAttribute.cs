namespace Swashbuckle;

[AttributeUsage(AttributeTargets.Method)]
public class DocumentAttribute(string _name)
    : Attribute
{
    public string Name => _name;
}