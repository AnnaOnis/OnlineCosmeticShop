
using CosmeticShop.DB.EF;
using CosmeticShop.DB.EF.Repositories;
using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Repositories;
using CosmeticShop.Domain.Services;
using CosmeticShop.WebAPI.Middlewares;
using IdentityPasswordHasher;
using JwtTokenGenerator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PaymentGateway;
using System.Globalization;
using CosmeticShop.WebAPI.Controllers;

namespace CosmeticShop.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            JwtConfig jwtConfig = builder.Configuration
                .GetRequiredSection("JwtConfig")
                .Get<JwtConfig>()!;
            if (jwtConfig is null)
            {
                throw new InvalidOperationException("JwtConfig is not configured");
            }
            builder.Services.AddSingleton(jwtConfig);
            builder.Services.AddSingleton<ITokenGenerationService, JwtTokenGenerationService>();

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { new CultureInfo("ru-RU") };
                options.DefaultRequestCulture = new RequestCulture("ru-RU");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                             new MySqlServerVersion(new Version(8, 0, 39))));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   IssuerSigningKey = new SymmetricSecurityKey(jwtConfig.SigningKeyBytes),
                   ValidateIssuerSigningKey = true,
                   ValidateLifetime = true,
                   RequireExpirationTime = true,
                   RequireSignedTokens = true,

                   ValidateAudience = true,
                   ValidateIssuer = true,
                   ValidAudiences = new[] { jwtConfig.Audience },
                   ValidIssuer = jwtConfig.Issuer
               };
           });
            builder.Services.AddAuthorization();


            builder.Services.AddSingleton<IAppPasswordHasher<Customer>, CustomerPasswordHasher>();
            builder.Services.AddSingleton<IAppPasswordHasher<User>, UserPasswordHasher>();

            builder.Services.AddScoped<ICartRepository, CartRepositoryEf>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepositoryEf>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepositoryEf>();
            builder.Services.AddScoped<IJwtTokenRepository, JwtTokenRepositoryEf>();
            builder.Services.AddScoped<IOrderRepository, OrderRepositoryEf>();  
            builder.Services.AddScoped<IPaymentRepository, PaymentRepositoryEf>();
            builder.Services.AddScoped<IProductRepository, ProductRepositoryEf>();
            builder.Services.AddScoped<IRewiewRepository, RewiewRepositoryEf>();
            builder.Services.AddScoped<IUserActionRepository, UserActionRepositoryEf>();
            builder.Services.AddScoped<IUserRepository, UserRepositoryEf>();

            builder.Services.AddScoped<CartService> ();
            builder.Services.AddScoped<CategoryService> ();
            builder.Services.AddScoped<CustomerService> ();
            builder.Services.AddScoped<OrderService> ();
            builder.Services.AddScoped<PaymentService> ();
            builder.Services.AddScoped<ProductService> ();
            builder.Services.AddScoped<ReviewService> ();
            builder.Services.AddScoped<UserService> ();
            builder.Services.AddScoped<JwtTokenService> ();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWorkEf>();
            builder.Services.AddScoped<IPaymentGateway, PaymentGatewayService>();


            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRequestLocalization();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseMiddleware<TokenValidationMiddleware>();
            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
