using Microsoft.Data.SqlClient;

namespace IsLabApp.Endpoints;

public static class DatabaseEndpoints
{
    public static void MapDatabaseEndpoints(this WebApplication app)
    {
        app.MapGet("/db/ping", async (IConfiguration config) =>
        {
            // 🔹 Получаем строку подключения из конфига
            var connectionString = config.GetConnectionString("Mssql");
            
            // 🔹 Проверка: есть ли строка подключения
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return Results.Json(new { 
                    status = "error", 
                    message = "Connection string 'Mssql' not configured in appsettings.json" 
                }, statusCode: 500);
            }

            try
            {
                // 🔹 Пытаемся подключиться к БД
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                
                // 🔹 Простая проверка: выполняем SELECT 1
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT 1";
                var result = await command.ExecuteScalarAsync();
                
                // 🔹 Успех
                return Results.Json(new { 
                    status = "ok", 
                    message = "Database connection successful",
                    serverVersion = connection.ServerVersion,
                    result = result?.ToString()
                });
            }
            catch (SqlException ex)
            {
                // 🔹 Ошибка подключения к БД
                return Results.Json(new { 
                    status = "error", 
                    message = "Failed to connect to database",
                    errorCode = ex.Number,
                    details = ex.Message 
                }, statusCode: 503);
            }
            catch (Exception ex)
            {
                // 🔹 Другая ошибка
                return Results.Json(new { 
                    status = "error", 
                    message = "Unexpected error during database ping",
                    details = ex.Message 
                }, statusCode: 500);
            }
        });
    }
}
