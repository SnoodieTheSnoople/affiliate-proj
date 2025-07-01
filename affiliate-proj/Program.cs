using System.Security.Cryptography;
using System.Text;
using affiliate_proj.Accessors.DatabaseAccessors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Supabase;

namespace affiliate_proj;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Logging.AddSimpleConsole(options => options.SingleLine = true);
        
        builder.Services.AddDbContext<PostgresDbContext>((sp, options) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var connectionString = config.GetValue<string>("Postgres:ConnectionString");
            options.UseNpgsql(connectionString);
        });

        // Add services to the container.

        // builder.Services.AddControllers();
        
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        // Exposing keys to test presence of keys.
        Console.WriteLine(builder.Configuration.GetValue<string>("Supabase:Url"));
        Console.WriteLine(builder.Configuration.GetValue<string>("Supabase:AnonPublicKey"));
        Console.WriteLine(builder.Configuration.GetValue<string>("Supabase:ServiceRoleKey"));
        Console.WriteLine(builder.Configuration.GetValue<string>("Postgres:ConnectionString"));
        Console.WriteLine(builder.Configuration.GetValue<string>("Supabase:JWTSecret"));
        
        // builder.Services.AddSingleton<SupabaseAccessor>(tmp => new SupabaseAccessor(
        //     builder.Configuration.GetValue<string>("Supabase:Url"),
        //     builder.Configuration.GetValue<string>("Supabase:AnonPublicKey"),
        //     builder.Configuration.GetValue<string>("Supabase:ServiceRoleKey"))
        // );
        
        builder.Services.AddSingleton<Supabase.Client>( _ =>
            new Supabase.Client(builder.Configuration.GetValue<string>("Supabase:Url"),
                builder.Configuration.GetValue<string>("Supabase:AnonPublicKey"),
                new SupabaseOptions {AutoRefreshToken = true})
        );

        builder.Services.AddScoped<SupabaseAccessor>();

        var bytes = Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Supabase:JWTSecret")!);
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            // options.Authority = builder.Configuration.GetValue<string>("Supabase:Url");
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = "authenticated",
                ValidIssuer = $"{builder.Configuration.GetValue<string>("Supabase:Url")}/auth/v1",
                IssuerSigningKey = GetSupabaseSigningKey(builder.Configuration, bytes)
            };
        });

        builder.Services.AddControllers();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();

        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    public static SecurityKey GetSupabaseSigningKey(IConfiguration configuration, byte[] jwtSecret)
    {
        // var publicKey = configuration.GetValue<string>("Supabase:JWTSecret");
        // if (!string.IsNullOrEmpty(publicKey))
        // {
        //     var rsa = RSA.Create();
        //     rsa.ImportFromPem(publicKey);
        //     return new RsaSecurityKey(rsa);
        // }
        return new SymmetricSecurityKey(jwtSecret);
    }
}