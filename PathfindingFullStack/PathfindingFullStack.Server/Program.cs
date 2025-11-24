
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
                    policy.WithOrigins("http://localhost:5174") // <-- frontend
                          .AllowAnyHeader()
                          .AllowAnyMethod());
            });

            List<Point> board = new List<Point>(PathfindingAlgorithm.FindPath(10, 10)); 



            builder.Services.AddControllers();
            
            builder.Services.AddOpenApi();
            
            var app = builder.Build();

            app.UseCors();
            app.UseDefaultFiles();
            app.MapStaticAssets();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

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
