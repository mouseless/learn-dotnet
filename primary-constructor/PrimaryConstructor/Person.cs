namespace PrimaryConstructor;

public class Person(int _length, float _mass)
{
    public string Status => (_mass > 120) && (_length < 190) ? "You are huge!" : "You are good bro!";
}