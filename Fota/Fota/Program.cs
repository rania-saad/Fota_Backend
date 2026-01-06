


//using Fota.Data;
using Fota.BusinessLayer.Repositories;
using Fota.DataLayer.DBContext;
using Fota.BusinessLayer.Services;
//using Fota.Services;
using Microsoft.EntityFrameworkCore;
using Fota.BusinessLayer.Interfaces;
using Fota.DataLayer.Repositories.Implementation;
using Fota.DataLayer.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Fota.BusinessLayer.Services;
using Fota.Services;
using Fota.DataLayer.DBContext;

namespace Fota
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // 2. AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // 3. DbContext
            builder.Services.AddDbContext<FOTADbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Rania")));

            builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<IDeveloperRepository, DeveloperRepository>();
            builder.Services.AddScoped<ITeamRepository, TeamRepository>();
            builder.Services.AddScoped<ITopicRepository, TopicRepository>();
            builder.Services.AddScoped<IBaseMessageRepository, BaseMessageRepository>();

            builder.Services.AddScoped<IDiagnosticRepository, DiagnosticRepository>();
            builder.Services.AddScoped<ISubscriberRepository, SubscriberRepository>();
            builder.Services.AddScoped<IDiagnosticSolutionRepository, DiagnosticSolutionRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();


            builder.Services.Configure<JwtSettings>(
                     builder.Configuration.GetSection("JwtSettings")
                    );
            builder.Services.AddSingleton(sp =>
                      sp.GetRequiredService<IOptions<JwtSettings>>().Value
                        );

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                      .AddEntityFrameworkStores<FOTADbContext>();


            builder.Services.AddAuthentication(options =>
            {
                // Read JWT token from request header and authenticate using Bearer scheme
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                // When [Authorize] fails, return 401 Unauthorized (instead of redirecting or sending 404)
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                // Set default scheme for the app
            })
.AddJwtBearer(options =>
{
    // Save token after successful login (useful if you need it later)
    options.SaveToken = true;

    // Allow HTTP for local development; set to true only when using HTTPS
    options.RequireHttpsMetadata = false;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Ensure the token issuer matches the configured issuer
        ValidateIssuer = true,

        // Ensure the token audience matches the configured audience
        ValidateAudience = true,

        // Ensure the token is not expired
        ValidateLifetime = true,

        // Ensure the signing key is valid
        ValidateIssuerSigningKey = true,

        // Read issuer and audience from configuration (appsettings.json)
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],

        // Validate token signature using the secret key
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)
        )
    };

    // Optional: Allow small clock difference when validating expiration time
    // options.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(2);
});


            // 4. MQTT Services
            builder.Services.AddSingleton<MqttService>();
            builder.Services.AddHostedService<MqttBackgroundService>();

            // 5. Controllers + Swagger
            builder.Services.AddLogging();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter 'Bearer' followed by a space and your token\n\nExample: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });


            var app = builder.Build();

            // Middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // تفعيل CORS
            app.UseCors("AllowAll");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
