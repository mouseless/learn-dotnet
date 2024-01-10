namespace PrimaryConstructor;

public class BookWorld(IBookService _bookService)
{
    public void ShowMeCards()
    {
        Console.WriteLine("Showing all books");

        foreach (var (book, index) in _bookService.GetBooks().Select((book, index) => (book, index)))
        {
            var card = new BookCard(book);
            Console.WriteLine($"{index + 1}. {card.DisplayString}");
        }
    }
}
