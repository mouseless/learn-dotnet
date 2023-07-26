using System.Collections.Generic;

namespace UnitTesting;

public class Forge
{
    public Forge()
    {
        CreatedSwords = new();
    }

    public Dictionary<int, string> CreatedSwords { get; set; }

    public void CreateASword(int length, string raw)
    {
        CreatedSwords.Add(length, raw);
    }
}
