using System.Reflection.Metadata;

namespace LearnReflection;

public class TypeNameParsing()
{
    Dictionary<string, (string question, Type answer)> _types = new()
    {
        {typeof(string).FullName!, ("I am the text itself.", typeof(string))},
        {typeof(int).FullName!, ("You want to count? You need me.", typeof(int))},
        {typeof(double).FullName!, ("I have numbers. I have comma.", typeof(double))},
    };

    public void Guess()
    {
        Console.WriteLine("*** Guessing the type ***");
        Random random = new();
        int index = random.Next(0, _types.Count);
        KeyValuePair<string, (string question, Type answer)> type = _types.ElementAt(index);
        Console.WriteLine($"{type.Value.question} What type am I?");
        var answer = Console.ReadLine();

        if (!TypeName.TryParse(answer.AsSpan(), out TypeName? parsed))
        {
            throw new ArgumentException("Invalid type name");
        }

        if (!_types.TryGetValue(parsed.FullName, out var value))
        {
            Console.WriteLine("Nope, try again.");
            return;
        }

        Console.WriteLine(@$"You find!:
        Name: {value.answer.Name},
        FullName: {value.answer.FullName},
        AssemblyQualifiedName: {value.answer.AssemblyQualifiedName}");
    }
}