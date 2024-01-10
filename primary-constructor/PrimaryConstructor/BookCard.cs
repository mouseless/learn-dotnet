namespace PrimaryConstructor;

public class BookCard(Book book)
    : Card(book)
{
    // It is not possible to use a parameter both as a field within a class and
    // as a parameter in the base class simultaneously.
    public override string DisplayString => $"{Title} - {Author} {Year} {Category}";
}
