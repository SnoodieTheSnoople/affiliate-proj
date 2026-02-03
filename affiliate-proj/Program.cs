using System.Text;
using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces;
using affiliate_proj.Application.Interfaces.Account;
using affiliate_proj.Application.Interfaces.Account.Affiliate.Code;
using affiliate_proj.Application.Interfaces.Account.Affiliate.Link;
using affiliate_proj.Application.Interfaces.Account.Creator;
using affiliate_proj.Application.Interfaces.Account.Rates;
using affiliate_proj.Application.Interfaces.CommissionAttribution;
using affiliate_proj.Application.Interfaces.Shopify;
using affiliate_proj.Application.Interfaces.Shopify.Data;
using affiliate_proj.Application.Interfaces.Shopify.Data.Factories;
using affiliate_proj.Application.Interfaces.Shopify.Data.Product;
using affiliate_proj.Application.Interfaces.Shopify.Webhook;
using affiliate_proj.Application.Interfaces.Shopify.Webhook.Conversion;
using affiliate_proj.Application.Interfaces.Store;
using affiliate_proj.Application.Services;
using affiliate_proj.Application.Services.Account;
using affiliate_proj.Application.Services.Account.Affiliate.Code;
using affiliate_proj.Application.Services.Account.Affiliate.Link;
using affiliate_proj.Application.Services.Account.Creator;
using affiliate_proj.Application.Services.Account.Rates;
using affiliate_proj.Application.Services.CommissionAttribution;
using affiliate_proj.Application.Services.Shopify;
using affiliate_proj.Application.Services.Shopify.Data;
using affiliate_proj.Application.Services.Shopify.Data.Factories;
using affiliate_proj.Application.Services.Shopify.Data.Product;
using affiliate_proj.Application.Services.Shopify.Webhook;
using affiliate_proj.Application.Services.Shopify.Webhook.Conversion;
using affiliate_proj.Application.Services.Store;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using ShopifySharp;
using ShopifySharp.Utilities;
using Supabase;
using IUserService = affiliate_proj.Application.Interfaces.Account.User.IUserService;
using UserService = affiliate_proj.Application.Services.Account.User.UserService;

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
        builder.Services.AddControllers();
        
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        // Exposing keys to test presence of keys.
        /*Console.WriteLine(builder.Configuration.GetValue<string>("Supabase:Url"));
        Console.WriteLine(builder.Configuration.GetValue<string>("Supabase:AnonPublicKey"));
        Console.WriteLine(builder.Configuration.GetValue<string>("Supabase:ServiceRoleKey"));
        Console.WriteLine(builder.Configuration.GetValue<string>("Postgres:ConnectionString"));
        Console.WriteLine(builder.Configuration.GetValue<string>("Supabase:JWTSecret"));*/
        
        builder.Services.AddSingleton<Supabase.Client>( _ =>
            new Supabase.Client(builder.Configuration.GetValue<string>("Supabase:Url"),
                builder.Configuration.GetValue<string>("Supabase:AnonPublicKey"),
                new SupabaseOptions {AutoRefreshToken = true})
        );

        builder.Services.AddScoped<SupabaseAccessor>();

        var bytes = Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Supabase:JWTSecret")!);
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
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

        builder.Services.AddMemoryCache();
        
        builder.Services.AddHttpContextAccessor();
        /* Account Service */
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ICreatorService, CreatorService>();
        builder.Services.AddScoped<IAccountHelper, AccountHelper>();
        builder.Services.AddScoped<ICommissionRatesService, CommissionRatesService>();
        builder.Services.AddScoped<ICommissionRatesRepository, CommissionRatesRepository>();
        builder.Services.AddScoped<IAffiliateLinkService, AffiliateLinkService>();
        builder.Services.AddScoped<IAffiliateLinkRepository, AffiliateLinkRepository>();
        builder.Services.AddScoped<IAffiliateCodeService, AffiliateCodeService>();
        builder.Services.AddScoped<IAffiliateCodeRepository, AffiliateCodeRepository>();
        
        /* Store Service */
        builder.Services.AddScoped<IStoreService, StoreService>();
        builder.Services.AddScoped<IShopifyStoreHelper, ShopifyStoreHelper>();
        
        /* Shopify Service*/
        builder.Services.AddScoped<IShopifyAuthService, ShopifyAuthService>();
        builder.Services.AddScoped<IShopifyDataService, ShopifyDataService>();
        builder.Services.AddScoped<IShopifyStateManager, ShopifyStateManager>();
        builder.Services.AddScoped<IShopifyProductService, ShopifyProductService>();
        builder.Services.AddScoped<IShopifyProductRepository, ShopifyProductRepository>();
        
        builder.Services.AddScoped<IGraphServiceFactory, GraphServiceFactory>();
        
        /* Webhook Service */
        builder.Services.AddScoped<IShopifyWebhookService, ShopifyWebhookService>();
        builder.Services.AddScoped<IShopifyWebhookRepository, ShopifyWebhookRepository>();

        builder.Services.AddScoped<IConversionService, ConversionService>();
        builder.Services.AddScoped<IConversionRepository, ConversionRepository>();
        
        /* CommissionAttribution Service */
        builder.Services.AddScoped<IEarnedCommissionService, EarnedCommissionService>();
        builder.Services.AddScoped<IEarnedCommissionRepository, EarnedCommissionRepository>();
        
        /* ShopifySharp Utilities */
        builder.Services.AddScoped<IShopifyRequestValidationUtility, ShopifyRequestValidationUtility>();
        builder.Services.AddScoped<IShopifyDomainUtility, ShopifyDomainUtility>();
        builder.Services.AddScoped<IShopifyOauthUtility, ShopifyOauthUtility>();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    public static SecurityKey GetSupabaseSigningKey(IConfiguration configuration, byte[] jwtSecret)
    {
        return new SymmetricSecurityKey(jwtSecret);
    }
}