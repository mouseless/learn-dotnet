using Stylecop.Analyzers.Abstracts;
using Stylecop.Analyzers.Attributes;
using Stylecop.Analyzers.Enums;

namespace Stylecop.Analyzers;

[Some]
[SomeOther]
public class ConventionallyCorrect : SomeBaseClass
{
    readonly string _field;
    public string AccessibleField => _field;
    static readonly string _staticField = string.Empty;

    public int Property { get; private set; }
    public string[] ArrayProperty { get; private set; }

    public ConventionallyCorrect()
        : base()
    {
        _field = string.Empty;
        Property = 0;

        ArrayProperty = [];
    }

    public ConventionallyCorrect(string field, int property)
    {
        _field = field;
        Property = property;

        ArrayProperty = [_field, AccessibleField, _staticField];
    }

    public string SomeMethod()
    {
        Property++;

        return _staticField;
    }

    public string SomeGenericMethod<TClass>(
        TClass parameter,
        string stringParameter,
        SomeEnum someEnum
    )
    {
        bool IsNull()
        {
            return parameter != null && someEnum == SomeEnum.Base;
        }

        return IsNull() ? stringParameter : _staticField;
    }
}
