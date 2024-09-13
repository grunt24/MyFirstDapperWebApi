using MyFirstDapperProject.Model;
using MyFirstDapperProject.Repository;
using MyFirstDapperProject.Repository.Interface;
using MyFirstDapperProject.Service;

namespace MyFirstDapperWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddCors(options =>
            {

                options.AddDefaultPolicy(builder =>
                {
                    builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
                           //.AllowAnyOrigin()
                           //.AllowAnyHeader()
                           //.AllowAnyMethod();
                });
            });

            builder.Services.AddControllers();

            //builder.Services.AddSingleton<DapperContext>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
