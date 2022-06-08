using Dolcecuore.Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace Dolcecuore.Infrastructure.FullTextSearch;

public static class FulltextCollectionExtensions
{
    private static IElasticClient _elasticClient;
    
    public static IServiceCollection AddFullTextSearch<TModel>(
        this IServiceCollection services,
        ElasticsearchOptions searchOptions)
        where TModel : class
    {
        var indexName = $"{typeof(TModel).Name.ToLower()}_index";

        if (_elasticClient == null)
        {
            var node = new Uri(searchOptions.Host);
            var settings = new ConnectionSettings(node)
                .DefaultMappingFor<TModel>(s => s.IndexName(indexName));

            _elasticClient = new ElasticClient(settings);
        }

        _elasticClient.Indices.Create(
            indexName,
            index => index
                .Map<TModel>(p => p.AutoMap()));

        services.AddSingleton(_elasticClient);
        services.AddTransient<IFullTextSearchService, FullTextSearchService>();
        
        return services;
    }
}