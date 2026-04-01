using IsLabApp.Endpoints;
using IsLabApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<InMemoryNotesService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Задание 3: диагностические эндпоинты
app.MapGet("/health", () =>
{
    return Results.Json(new
    {
        status = "ok",
        timestamp = DateTime.UtcNow,
        localTime = DateTime.Now
    });
});

app.MapGet("/version", (IConfiguration config) =>
{
    return Results.Json(new
    {
        application = config["App:Name"] ?? "Unknown",
        version = config["App:Version"] ?? "0.0.0"
    });
});

// Задание 4: эндпоинты заметок
app.MapNotesEndpoints();

app.MapDatabaseEndpoints();
app.Run();
