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

// Задание 3: /health
app.MapGet("/health", () => Results.Json(new { status = "ok", timestamp = DateTime.UtcNow }));

// Задание 3: /version
app.MapGet("/version", (IConfiguration cfg) => 
{
    var name = cfg["App:Name"] ?? "Unknown";
    var ver = cfg["App:Version"] ?? "0.0.0";
    return Results.Json(new { application = name, version = ver });
});

// Задание 4: заметки
app.MapNotesEndpoints();

// Задание 5: база данных
app.MapDatabaseEndpoints();

app.Run();
