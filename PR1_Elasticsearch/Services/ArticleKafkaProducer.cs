using Confluent.Kafka;
using PR1_Elasticsearch.Models;
using System.Text.Json;

namespace PR1_Elasticsearch.Services;

public class ArticleKafkaProducer
{
    private readonly IProducer<string, string> _producer;

    public ArticleKafkaProducer(ProducerConfig config)
    {
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task PublishAsync(ArticleDocument article)
    {
        var json = JsonSerializer.Serialize(article);

        await _producer.ProduceAsync("articles", new Message<string, string>
        {
            Key = article.Id.ToString(),
            Value = json
        });
    }
}