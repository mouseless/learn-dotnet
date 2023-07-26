namespace UnitTesting;

public class Sword: ISword
{
    readonly Mold _mold;
    readonly Raw _raw;

    public Sword(Mold mold, Raw raw)
    {
        _mold = mold;
        _raw = raw;
    }

    public Mold Mold => _mold;
    public Raw Raw => _raw;

    public override bool Equals(object other)
    {
        return Mold == ((Sword)other).Mold &&
               Raw == ((Sword)other).Raw;
    }
}
