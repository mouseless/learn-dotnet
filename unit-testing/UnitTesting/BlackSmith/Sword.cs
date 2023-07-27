namespace UnitTesting;

public class Sword: ISword
{
    readonly Raw _raw;

    public Sword(Raw raw)
    {
        _raw = raw;
    }

    public string Raw => _raw.Name;

    public override bool Equals(object other)
    {
        return Raw == ((Sword)other).Raw;
    }
}
