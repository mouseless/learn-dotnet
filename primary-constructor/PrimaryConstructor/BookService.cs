namespace PrimaryConstructor;

public class BookService : IBookService
{
    public IEnumerable<Book> GetBooks() =>
        [
            new("Harry Potter and Philosopher's Stone", "J. K. Rowling", 1997, "Fantastic"),
            new("Harry Potter and Chamber of Secrets", "J. K. Rowling", 1998, "Fantastic"),
            new("Harry Potter and Prisoner of Azkaban", "J. K. Rowling", 1999, "Fantastic")
        ];
}
