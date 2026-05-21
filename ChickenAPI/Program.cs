using Microsoft.EntityFrameworkCore;
using ChickenAPI.Model;

namespace ChickenAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<FarmDbContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING")));

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();


            var app = builder.Build();

            // Attempt to initialize the database with retry logic
            RetryDatabaseConnection(app.Services);

            // Configure the HTTP request pipeline.
            if (true)
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "ChickenAPI v1");
                    options.RoutePrefix = "swagger";
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            
            app.Run();
        }

        private static void RetryDatabaseConnection(IServiceProvider services, int maxRetries = 10, int delayMs = 3000)
        {
            using (var scope = services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<FarmDbContext>();
                
                for (int i = 0; i < maxRetries; i++)
                {
                    try
                    {
                        if (dbContext.Database.CanConnect())
                        {
                            Console.WriteLine("Database connection successful!");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Database connection attempt {i + 1}/{maxRetries} failed: {ex.Message}");
                    }

                    if (i < maxRetries - 1)
                    {
                        Thread.Sleep(delayMs);
                    }
                }

                Console.WriteLine("Failed to connect to database after retries, but continuing startup...");
            }
        }
    }
}