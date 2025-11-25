
namespace PathfindingFullStack.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                    policy.WithOrigins("http://localhost:5173") // <-- frontend
                          .AllowAnyHeader()
                          .AllowAnyMethod());
            });

            

            builder.Services.AddControllers();
            
            builder.Services.AddOpenApi();
            
            var app = builder.Build();

            app.MapPost("/api/path", async (Data payload) =>
            {
                Console.WriteLine($"START: {payload.start?.XPosition}, {payload.start?.YPosition}");
                return Results.Ok(new { received = payload });
            });
            

            List<Point> board = new List<Point>(PathfindingAlgorithm.FindPath(10, 10));
            app.UseCors();
            app.UseDefaultFiles();
            app.MapStaticAssets();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseAuthorization();


            app.MapControllers();
         

            app.MapGet("/api/board", () =>
            {
                return board;
            });
            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
