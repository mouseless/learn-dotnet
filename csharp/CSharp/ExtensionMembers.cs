namespace CSharp;

public static class ExtensionMembers
{
    extension<T>(IEnumerable<T> source)
    {
        public bool IsEmpty => !source.Any();
    }

    extension<T>(IEnumerable<T>)
    {
        public static IEnumerable<T> Combine(IEnumerable<T> first, IEnumerable<T> second) => first.Concat(second);
        public static IEnumerable<T> Identity => [];
    }
}