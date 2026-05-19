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

        public async Task IndexAsync(IEnumerable<ArticleDocument> documents)
        {
            // Bulk-индексация - предпочтительный способ записи
            var response = await _client.BulkAsync(b => b
                .Index("articles")
                .IndexMany(documents)
            );

            if (response.Errors)
            {
                throw new InvalidOperationException("Ошибка при индексации документов");
            }
        }

        public async Task<IReadOnlyCollection<ArticleDocument>> SearchAsync(string query)
        {
            var response = await _client.SearchAsync<ArticleDocument>(s => s
                .Indices("articles")
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Content)
                        .Query(query)
                    )
                )
            );

            return response.Documents;
        }
    }
}
