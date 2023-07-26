namespace UnitTesting;

public class Forge : IForge
{
    public List<ISword> MakeSword(Mold mold, List<Raw> raws)
    {
        List<ISword> result = new();

        raws.ForEach(raw => result.Add(new Sword(mold, raw)));

        return result;
    }
}
