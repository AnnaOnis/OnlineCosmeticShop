
using CosmeticShop.DB.EF;
using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Services;
using IdentityPasswordHasher;
using JwtTokenGenerator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
            builder.Services.AddSingleton<ITokenService, JwtTokenService>();

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


            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
