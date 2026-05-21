using Confluent.Kafka;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using PR1_Elasticsearch.Services;

namespace PR1_Elasticsearch
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var settings = new ElasticsearchClientSettings(new Uri("http://localhost:9200"))
                .Authentication(new BasicAuthentication("elastic", "elastic_password"))
                .DefaultIndex("articles");

            builder.Services.AddSingleton(new ElasticsearchClient(settings));

            #region -- Kafka producer

            builder.Services.AddSingleton(new ProducerConfig
            {
                BootstrapServers = "localhost:9094"
            });

            builder.Services.AddSingleton<KafkaProducerService>();
            builder.Services.AddSingleton<ArticleKafkaProducer>();


            #endregion


            #region -- Kafka consumer

            //builder.Services.AddSingleton<IConsumer<string, string>>(_ =>
            //{
            //    var config = new ConsumerConfig
            //    {
            //        BootstrapServers = "localhost:9094",
            //        GroupId = "article-indexer",
            //        AutoOffsetReset = AutoOffsetReset.Earliest,
            //        EnableAutoCommit = false
            //    };

            //    return new ConsumerBuilder<string, string>(config)
            //    .Build();
            //});

            //builder.Services.AddHostedService<KafkaToElasticHostedService>();

            #endregion


            builder.Services.AddScoped<ArticleSearchService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ArticleSearchService>();
                await service.EnsureIndexAsync();
            }

            app.Run();
        }
    }
}