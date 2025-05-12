using System;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sufra_MVC.Repositories;
using Sufra_MVC.Data;
using Sufra_MVC.Infrastructure.Hubs;
using Sufra_MVC.Infrastructure.Services;
using Sufra_MVC.Services.IServices;
using Sufra_MVC.Services.Services;
using sufra.Sufra.Emps.Infrastructure.Repositories;
using Services.IServices;
using System.Text.Json.Serialization;
using Sufra_MVC.Repositories.IRepositories;

namespace Sufra_MVC
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
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.ContentType = "application/json";
                            var result = JsonSerializer.Serialize(new { message = "Admins only!" });
                            return context.Response.WriteAsync(result);
                        }
                    };
                });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
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

            builder.Services.AddScoped<ISearchServices, SearchServices>();

            builder.Services.AddScoped<IReservationServices, ReservationServices>();
            builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

            builder.Services.AddScoped<ICartServices, CartServices>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();

            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderServices, OrderServices>();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Map your SignalR hub so clients can connect to it
            app.MapHub<SupportHub>("/support");
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
