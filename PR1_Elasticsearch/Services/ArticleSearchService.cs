using Elastic.Clients.Elasticsearch;
using PR1_Elasticsearch.Models;

namespace PR1_Elasticsearch.Services
{
    public class ArticleSearchService
    {
        private readonly ElasticsearchClient _client;

        public ArticleSearchService(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task EnsureIndexAsync()
        {
            var exists = await _client.Indices.ExistsAsync("articles");
            if (exists.Exists)
            {
                await _client.Indices.DeleteAsync("articles");
            }

            var create = await _client.Indices.CreateAsync("articles", c => c
                .Mappings(m => m
                    .Properties<ArticleDocument>(p => p
                        .Text(t => t.Name)
                        .Text(t => t.Content)
                    )
                )
            );

            if (!create.IsValidResponse)
                throw new InvalidOperationException("Не удалось создать индекс articles");
        }

        public async Task IndexAsync(IEnumerable<ArticleDocument> documents)
        {
            var response = await _client.BulkAsync(b => b
                .Index("articles")
                .IndexMany(documents)
            );

            if (response.Errors)
                throw new InvalidOperationException("Ошибка при индексации документов");
        }

        public async Task<IReadOnlyCollection<ArticleDocument>> SearchAsync(string query)
        {
            var response = await _client.SearchAsync<ArticleDocument>(s => s
                .Index("articles")
                .Query(q => q
                    .Bool(b => b
                        .Should(
                            sh => sh.Match(m => m.Field(f => f.Name).Query(query)),
                            sh => sh.Match(m => m.Field(f => f.Content).Query(query))
                        )
                    )
                )
            );

            return response.Documents;
        }
    }
}