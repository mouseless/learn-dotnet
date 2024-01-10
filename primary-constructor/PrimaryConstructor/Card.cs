namespace PrimaryConstructor;

public abstract class Card(Book book)
{
    protected string Title { get; } = book.Title;
    protected string Author { get; } = book.Author;
    protected string Category { get; } = book.Category;
    protected int Year { get; } = book.Year;

    public virtual string DisplayString => $"{Title} - {Author}";
}
