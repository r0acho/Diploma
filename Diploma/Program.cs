using System.Text.Json;

namespace Diploma
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Use(async (context, next) =>
            {
                string? host = context.Request.Host.Value;
                string? path = context.Request.Path.Value;
                string? query = context.Request.QueryString.Value;
                var response = new { host = host, path = path, query = query };
                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response)
                    );
                await next();
            });

            app.Run();
        }
    }
}