using System.Linq.Expressions;

namespace Dolcecuore.Infrastructure.FullTextSearch;

public interface IFullTextSearchService
{
    Task Index<TDocument>(TDocument value) where TDocument : class;
    Task Update<TDocument>(TDocument value, Guid id) where TDocument : class;
    Task Delete<TDocument>(Guid id) where TDocument : class;
    
    Task<IEnumerable<TDocument>> Query<TDocument>(
        Expression<Func<TDocument, object>> field,
        string query)
        where TDocument : class;
}