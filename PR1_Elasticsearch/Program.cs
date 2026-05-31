using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.EntityFrameworkCore;
using PR1_Elasticsearch.Data;
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

            // Elasticsearch client
            var settings = new ElasticsearchClientSettings(new Uri("http://localhost:9200"))
                .Authentication(new BasicAuthentication("elastic", "elastic_password"))
                .DefaultIndex("articles");

            builder.Services.AddSingleton(new ElasticsearchClient(settings));

            // MariaDB DbContext
            var connectionString = "Server=localhost;Port=3306;Database=articles_db;User=articles_user;Password=articles_password;";

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString, Pomelo.EntityFrameworkCore.MySql.SqlServerVersion.AutoDetect(connectionString)));

            // Kafka Producer УДАЛЁН для ПР4 (CDC вместо producer)
            // builder.Services.AddSingleton(new ProducerConfig { BootstrapServers = "localhost:9094" });
            // builder.Services.AddSingleton<KafkaProducerService>();
            // builder.Services.AddSingleton<ArticleKafkaProducer>();

            // Kafka Consumer УДАЛЁН для ПР4 (Kafka Connect вместо consumer)
            // builder.Services.AddSingleton<IConsumer<string, string>>(_ => { ... });
            // builder.Services.AddHostedService<KafkaToElasticHostedService>();

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

            // Создаём таблицу в MariaDB при старте
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated();
            }

            app.Run();
        }
    }
}