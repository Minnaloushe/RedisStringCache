using StackExchange.Redis;
using StringCacheAPI.Services;

namespace StringCacheAPI
{
    public class Program
    {
        //TODO Add error handling
        //TODO Add logging
        //TODO Add tests
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            ConfigureDependencies(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureDependencies(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IConnectionMultiplexer>(cfg => ConnectionMultiplexer.Connect("redis"));
            builder.Services.AddScoped(cfg => cfg.GetService<IConnectionMultiplexer>().GetDatabase());
            builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
        }
    }
}