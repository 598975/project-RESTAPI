using DAT250_REST.Data;
using DAT250_REST.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAuthentication();
        builder.Services.AddIdentityApiEndpoints<User>()
            .AddEntityFrameworkStores<AppDBContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddDbContext<AppDBContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();



        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapIdentityApi<User>();
        app.MapPost("/logout", async (SignInManager<User> signInManager) =>
        {
            await signInManager.SignOutAsync();
            return Results.Ok();
        }).RequireAuthorization();

        app.MapGet("/pingauth", (ClaimsPrincipal user) =>
        {
            var email = user.FindFirstValue(ClaimTypes.Email);
            return Results.Json(new { Email = email });

        }).RequireAuthorization();

        app.MapControllers();
        //app.UseEndpoints( endpoints =>
        //{
        //    endpoints.MapControllers();
        //});
        app.MapFallbackToFile("/index.html");

        app.Run();
    }
}