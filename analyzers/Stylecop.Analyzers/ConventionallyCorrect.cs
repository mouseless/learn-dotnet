using Stylecop.Analyzers.Abstracts;
using Stylecop.Analyzers.Attributes;
using Stylecop.Analyzers.Enums;

namespace Stylecop.Analyzers;

[Some]
[SomeOther]
public class ConventionallyCorrect : SomeBaseClass
{
    readonly string field;
    public string AccessibleField => _field;
    static readonly string StaticField = string.Empty;

    public int Property { get; private set; }
    public string[] ArrayProperty { get; private set; }

    public ConventionallyCorrect()
        : base()
    {
        _field = string.Empty;
        Property = 0;

        ArrayProperty = Array.Empty<string>();
    }

    public ConventionallyCorrect(string field, int property)
    {
        _field = field;
        Property = property;

        ArrayProperty = new string[3] { _field, AccessibleField, StaticField };
    }

    public string SomeMethod()
    {
        Property++;

        return StaticField;
    }

    public string SomeGenericMethod<TClass>(
        TClass parameter,
        string stringParameter,
        SomeEnum someEnum
    )
    {
        Func<bool> isNull = () =>
        {
            if (parameter != null && someEnum == SomeEnum.Base)
            {
                return true;
            }

            return false;
        };

        return isNull() ? stringParameter : StaticField;
    }
}
