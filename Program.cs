using System;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using Sufra.Services.IServices;
using Sufra.Repositories.IRepositories;
using Sufra.Infrastructure.Services;
using Sufra.Repositories.Repositories;
using Sufra.Services.Services;
using Sufra.Data;
using Sufra.Configuration;

namespace Sufra
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var env = builder.Environment;
            
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            builder.Services.AddDbContext<Sufra_DbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
                if (env.IsDevelopment()) options.EnableSensitiveDataLogging();
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["TokenSettings:Issuer"],
                        ValidAudience = builder.Configuration["TokenSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenSettings:Key"])),

                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            var result = JsonSerializer.Serialize(new { message = "You must be logged in." });
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.ContentType = "application/json";
                            var result = JsonSerializer.Serialize(new { message = "You are not allowed to access this resource." });
                            return context.Response.WriteAsync(result);
                        }
                    };
                });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:5173", "https://sufraa.vercel.app")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
            builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
            builder.Services.Configure<SupportSettings>(builder.Configuration.GetSection("SupportSettings"));


            builder.Services.AddScoped<ITokenService,TokenService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            builder.Services.AddScoped<IQRCodeService, QRCodeService>();
            builder.Services.AddScoped<IEmailServices, EmailServices>();


            builder.Services.AddScoped<IAdminServices, AdminServices>();
            builder.Services.AddScoped<ISufraEmpRepository, SufraEmpRepository>();

            builder.Services.AddScoped<ICustomerServices, CustomerServices>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            
            builder.Services.AddScoped<IRestaurantServices, RestaurantServices>();
            builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();

            builder.Services.AddScoped<IRestaurantManagerRepository, RestaurantManagerRepository>();

            builder.Services.AddScoped<IMenuSectionServices, MenuSectionServices>();
            builder.Services.AddScoped<IMenuSectionRepository, MenuSectionRepository>();

            builder.Services.AddScoped<IMenuItemServices, MenuItemServices>();
            builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();


            builder.Services.AddScoped<IReservationServices, ReservationServices>();
            builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

            builder.Services.AddScoped<ICartServices, CartServices>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();

            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderServices, OrderServices>();

            builder.Services.AddScoped<ICuisineRepository, CuisineRepository>();
            builder.Services.AddScoped<ICuisineServices, CuisineServices>();

            builder.Services.AddScoped<IDistrictRepository, DistrictRepository>();
            builder.Services.AddScoped<IDistrictServices, DistrictServices>();

            var app = builder.Build();

            app.UseCors("AllowFrontend");
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
