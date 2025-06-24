using affiliate_proj.Accessors.DatabaseAccessors;

namespace affiliate_proj;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        // Exposing keys to test presence of keys.
        Console.WriteLine(builder.Configuration.GetValue<string>("Supabase:Url"));
        Console.WriteLine(builder.Configuration.GetValue<string>("Supabase:AnonPublicKey"));
        Console.WriteLine(builder.Configuration.GetValue<string>("Supabase:ServiceRoleKey"));
        
        builder.Services.AddSingleton<SupabaseAccessor>(tmp => new SupabaseAccessor(
            builder.Configuration.GetValue<string>("Supabase:Url"),
            builder.Configuration.GetValue<string>("Supabase:AnonPublicKey"),
            builder.Configuration.GetValue<string>("Supabase:ServiceRoleKey"))
        );


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