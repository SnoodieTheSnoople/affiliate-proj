using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces;
using affiliate_proj.Application.Services;
using Microsoft.EntityFrameworkCore;

namespace affiliate_proj;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Logging.AddSimpleConsole(options => options.SingleLine = true);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        // Exposing keys to test presence of keys.
        // Console.WriteLine(builder.Configuration.GetValue<string>("Supabase:Url"));
        // Console.WriteLine(builder.Configuration.GetValue<string>("Supabase:AnonPublicKey"));
        // Console.WriteLine(builder.Configuration.GetValue<string>("Supabase:ServiceRoleKey"));
        // Console.WriteLine(builder.Configuration.GetValue<string>("Postgres:ConnectionString"));
        
        // builder.Services.AddSingleton<SupabaseAccessor>(tmp => new SupabaseAccessor(
        //     builder.Configuration.GetValue<string>("Supabase:Url"),
        //     builder.Configuration.GetValue<string>("Supabase:AnonPublicKey"),
        //     builder.Configuration.GetValue<string>("Supabase:ServiceRoleKey"))
        // );

        builder.Services.AddDbContext<SupabaseAccessor>((sp, options) =>
        {
            var config =  sp.GetRequiredService<IConfiguration>();
            var connectionString = config.GetValue<string>("Postgres:ConnectionString");
            
            options.UseNpgsql(connectionString);
        });

        builder.Services.AddScoped<IAccountService, AccountService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}