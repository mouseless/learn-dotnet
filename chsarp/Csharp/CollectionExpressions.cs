namespace Csharp;

public class CollectionExpressions
{
    public void EmptyCollectionInitialization()
    {
        int[] intArray = [];
        List<int> intList = [];
        Dictionary<string, int> dictionary = [];
    }

    public void CollectionInitialization()
    {
        int[] intArray = [1, 2, 3, 4, 5];
        List<int> intList = [1, 2, 3, 4, 5];
        Dictionary<string, string> dictionary = new() { ["key"] = "value" };

        int[] intCollection = [.. intArray, .. intList];
        int[] anotherIntCollection = [.. intCollection, 1, 2, 3, 4, 5];

        Console.WriteLine(anotherIntCollection);
    }

    public void CallMethods()
    {
        SomeMethod("arg1", "arg2", "arg3");

        SomeMethod(args: ["arg1", "arg2", "arg3"]);
    }

    public void SomeMethod(params string[] args) { }
    public void SomeOtherMethod(string[] args) { }
}
