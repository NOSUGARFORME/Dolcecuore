using System.Linq.Expressions;
using Nest;

namespace Dolcecuore.Infrastructure.FullTextSearch;

public class FullTextSearchService : IFullTextSearchService
{
    private readonly IElasticClient _elasticClient;

    public FullTextSearchService(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public Task Index<TDocument>(TDocument value) where TDocument : class
        => _elasticClient.IndexDocumentAsync(value);

    public Task Update<TDocument>(TDocument value, Guid id) where TDocument : class
        => _elasticClient.IndexAsync(value, i => i
            .Index($"{typeof(TDocument).Name.ToLower()}_index")
            .Id(id)
            .Refresh(Elasticsearch.Net.Refresh.True));

    public Task Delete<TDocument>(Guid id) where TDocument : class
        => _elasticClient.DeleteAsync<TDocument>(id, d =>
            d.Index($"{typeof(TDocument).Name.ToLower()}_index"));

    public async Task<IEnumerable<TDocument>> Query<TDocument>(Expression<Func<TDocument, object>> field, string query)
        where TDocument : class
    {
        var result = await _elasticClient
            .SearchAsync<TDocument>(s => s
                .Query(q => q
                    .MatchPhrase(m => m.Field(field)
                        .Query(query))));

        return result.Documents;
    }
}