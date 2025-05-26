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
using Sufra.Infrastructure.Hubs;
using Sufra.Services.Services;
using Sufra.Data;

namespace Sufra
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            builder.Services.AddSwaggerGen();

            builder.Services.AddSignalR();

            builder.Services.AddDbContext<Sufra_DbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
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
                    policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });



            builder.Services.AddScoped<JwtServices>();
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

            // Map your SignalR hub so clients can connect to it
            app.MapHub<ChatHub>("/chat");
            app.UseCors("AllowFrontend");
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
