using Minio;
using Minio.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MinIO client
builder.Services.AddSingleton<IMinioClient>(sp =>
{
    var minio = new MinioClient()
        .WithEndpoint("minio:9000")
        .WithCredentials("minio_access_key", "minio_secret_key")
        .Build();
    return minio;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();