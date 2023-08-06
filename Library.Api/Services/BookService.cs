using Library.Api.Data;
using Library.Api.Models;
using MongoDB.Driver;

namespace Library.Api.Services;

public class BookService : IBookService
{
    private readonly IMongoCollection<Book> _booksCollection;

    public BookService(IMongoDbClientFactory connectionFactory)
    {
        _booksCollection = connectionFactory.GetDatabase("test").GetCollection<Book>("books");
        Task.Run(() => EnsureIndexes()).Wait(); // Figure out a better way of doing this 
    }

    public async Task<bool> CreateAsync(Book book)
    {
        var existingBook = await GetByIsbnAsync(book.Isbn);
        if (existingBook is not null) return false;

        await _booksCollection.InsertOneAsync(book);
        return true;
    }

    public async Task<Book?> GetByIsbnAsync(string isbn)
    {
        var result = await _booksCollection.Find(b => b.Isbn == isbn).ToListAsync();
        return result?.FirstOrDefault();
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _booksCollection.Find(_ => true).ToListAsync();
    }

    public async Task<IEnumerable<Book>> SearchByTitleAsync(string searchTerm)
    {
        return await _booksCollection.Find(b => b.Title.ToLower().Contains(searchTerm.ToLower())).ToListAsync();
    }

    public async Task<bool> UpdateAsync(Book book)
    {
        var result = await _booksCollection.ReplaceOneAsync(b => b.Isbn == book.Isbn, book);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string isbn)
    {
        var result = await _booksCollection.DeleteOneAsync(b => b.Isbn == isbn);
        return result.DeletedCount > 0;
    }

    public async Task<IEnumerable<string?>> EnsureIndexes()
    {
        var options = new CreateIndexOptions { Unique = true };
        var bookBuilder = Builders<Book>.IndexKeys;
        var indexModel = new CreateIndexModel<Book>(bookBuilder.Ascending(b => b.Isbn), options);
        var titleModel = new CreateIndexModel<Book>(bookBuilder.Text(b => b.Title));
        var result = await _booksCollection.Indexes.CreateManyAsync(new[] { indexModel, titleModel });
        return result;
    }
}
