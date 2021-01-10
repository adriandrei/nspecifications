using NSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpecificationPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var repo = new Repository();

            Console.WriteLine($"PG13 Books: {string.Join(", ", repo.Find(Book.PG13Books).Select(t => t.Title))}");
            Console.WriteLine($"Books by Martin: {string.Join(", ", repo.Find(Book.BooksBy("Martin")).Select(t => t.Title))}");
            Console.WriteLine($"NonR Books: {string.Join(", ", repo.Find(Book.NonRBooks).Select(t => t.Title))}");

            Console.ReadKey();
        }
    }

    class Repository
    {

        private List<Book> _books = new List<Book>()
        {
            new Book("The Lord of the Rings", "Tolkien", Rating.PG13),
            new Book("The Hobbit", "Tolkien", Rating.PG13),
            new Book("A song of Ice and Fire", "Martin", Rating.M)
        };

        public IReadOnlyList<Book> Find(ISpecification<Book> spec)
        {
            return _books.Where(spec.Expression.Compile()).ToList();
        }
    }


    class Book
    {
        public Book(string title, string author, Rating rating)
        {
            this.Title = title;
            this.Author = author;
            this.Rating = rating;
        }

        public string Title { get; set; }
        public string Author { get; set; }
        public Rating Rating { get; set; }

        public static readonly ISpecification<Book> PG13Books = new Spec<Book>(t => t.Rating == Rating.PG13);

        public static readonly ISpecification<Book> NonRBooks = new Spec<Book>(t => t.Rating != Rating.R);

        public static ISpecification<Book> BooksBy(string author)
        {
            return new Spec<Book>(t => t.Author == author);
        }
    }

    enum Rating
    {
        PG13,
        R,
        M,
        G
    }
}
