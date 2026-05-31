//using Confluent.Kafka;
//using PR1_Elasticsearch.Models;

//namespace PR1_Elasticsearch.Services;

//public class KafkaProducerService
//{
//    private readonly IProducer<string, string> _producer;
//    private const string Topic = "articles";

//    public KafkaProducerService(ProducerConfig config)
//    {
//        _producer = new ProducerBuilder<string, string>(config).Build();
//    }

//    public async Task ProduceAsync(ArticleDocument article)
//    {
//        var json = System.Text.Json.JsonSerializer.Serialize(article);

//        var message = new Message<string, string>
//        {
//            Key = article.Id.ToString(),
//            Value = json
//        };

//        await _producer.ProduceAsync(Topic, message);
//    }
//}