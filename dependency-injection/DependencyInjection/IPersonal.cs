namespace DependencyInjection;

public interface IPersonal
{
    public string Name => default!;
}

public enum ServiceImplementation
{
    PersonalA,
    PersonalB
}