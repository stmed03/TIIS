using Minio;
using Minio.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<IMinioClient>(sp =>
{
    var minio = new MinioClient()
        .WithEndpoint("localhost:9000")
        .WithCredentials("minio_access_key", "minio_secret_key")
        .Build();
    return minio;
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();