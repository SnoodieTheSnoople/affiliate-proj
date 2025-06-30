using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces;
using affiliate_proj.Application.Services;
using affiliate_proj.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

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

        builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<SupabaseAccessor>()
            .AddDefaultTokenProviders();

        builder.Services.AddSingleton<IEmailSender<User>, NoOpEmailSender>();

        builder.Services.AddAuthorization();

        var app = builder.Build();

        app.MapIdentityApi<User>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();

        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}