using Microsoft.Extensions.Logging;

namespace CSharp;

public class CollectionExpressions(ILogger<CollectionExpressions> _logger)
{
    public void EmptyCollectionInitialization()
    {
        int[] intArray = [];
        _logger.LogInformation($"Empty Array initialization: {string.Join(',', intArray)}");

        List<int> intList = [];
        _logger.LogInformation($"Empty List initialization: {string.Join(',', intList)}");

        Dictionary<string, int> dictionary = [];
        _logger.LogInformation($"Empty Dictionary initialization: {string.Join(',', dictionary.Keys)} {string.Join(',', dictionary.Values)}");
    }

    public void CollectionInitialization()
    {
        int[] intArray = [1, 2, 3, 4, 5];
        _logger.LogInformation($"Array initialization: {string.Join(',', intArray)}");

        List<int> intList = [6, 7, 8, 9, 10];
        _logger.LogInformation($"List initialization: {string.Join(',', intList)}");

        Dictionary<string, string> dictionary = new() { ["key"] = "someValue" };
        _logger.LogInformation($"Dictionary initialization: {string.Join(',', dictionary.Keys)} {string.Join(',', dictionary.Values)}");

        int[] intCollection = [.. intArray, .. intList];
        _logger.LogInformation($"Array initialization with list and array spread elements: {string.Join(',', intCollection)}");

        int[] anotherIntCollection = [.. intCollection, 1, 2, 3, 4, 5];
        _logger.LogInformation($"Array initialization with spread element and members: {string.Join(',', anotherIntCollection)}");
    }

    public void CallMethods()
    {
        SomeMethod("arg1", "arg2", "arg3");

        SomeMethod(args: ["arg1", "arg2", "arg3"]);
    }

    public void SomeMethod(params string[] args)
    {
        _logger.LogInformation($"SomeMethod is called with args: {string.Join(',', args)}");
    }

    public void SomeOtherMethod(string[] args)
    {
        _logger.LogInformation($"SomeOtherMethod is called with args: {string.Join(',', args)}");
    }
}
