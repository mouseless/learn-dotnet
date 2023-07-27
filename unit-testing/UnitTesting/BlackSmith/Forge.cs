namespace UnitTesting;

public class Forge : IForge
{
    private readonly ITool _tool;

    public Forge(ITool tool)
    {
        _tool = tool;
    }

    public ISword MakeSword(Raw raw)
    {
        if(raw == null) throw new ArgumentNullException(nameof(raw));

        _tool.Use();

        return new Sword(raw);
    }
}
