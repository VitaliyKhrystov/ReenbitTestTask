using Microsoft.AspNetCore.Http;
using WebAppReenbitTest.Services;

namespace WebAppReenbitTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.Bind("BlobCredential", new BlobCredential());
            builder.Services.AddControllers();
            builder.Services.AddScoped<FileService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseCors((policy) => { policy.AllowAnyHeader(); policy.AllowAnyMethod(); policy.AllowAnyOrigin(); });
            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}