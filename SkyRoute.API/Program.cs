using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SkyRoute.API.Exceptions;
using SkyRoute.Application;
using SkyRoute.Application.DTOs;
using SkyRoute.Application.Interfaces;
using SkyRoute.Application.Services;
using SkyRoute.Infraestructure.Persistence;
using SkyRoute.Infraestructure.Persistence.Repositories;
using SkyRoute.Infraestructure.Providers.BudgetWings;
using SkyRoute.Infraestructure.Providers.GlobalAir;
using SkyRoute.Infrastructure.Cache;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SkyRoute.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ── Database ──────────────────────────────────────────────────────────────
            builder.Services.AddDbContext<SkyRouteDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("SkyRouteDb"),
                    sql => sql.MigrationsAssembly(typeof(SkyRouteDbContext).Assembly.FullName)));

            // ── Application + AutoMapper (Application profiles + API profiles) ────────
            builder.Services.AddApplication(typeof(Program).Assembly);

            // ── CORS (open for development; tighten in production) ────────────────────
            builder.Services.AddCors(options =>
                options.AddDefaultPolicy(policy =>
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader()));

            // ── Controllers + JSON ────────────────────────────────────────────────────
            builder.Services.AddControllers()
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                });

            // ── RFC 7807 ProblemDetails ───────────────────────────────────────────────
            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<SkyRouteExceptionHandler>();

            // ── Swagger / OpenAPI ─────────────────────────────────────────────────────
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title       = "SkyRoute API",
                    Version     = "v1",
                    Description = "SkyRoute Flight Search & Booking API — multi-provider flight aggregation platform."
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                    options.IncludeXmlComments(xmlPath);
            });

            // ── Caching ───────────────────────────────────────────────────────────────
            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<IFlightCacheProvider, FlightCacheProvider>();

            // ── Infrastructure: provider reservation (in-memory, per-process) ─────────
            builder.Services.AddSingleton<IProviderReservationRepository, InMemoryProviderReservationRepository>();

            // ── Infrastructure: repositories ──────────────────────────────────────────
            builder.Services.AddScoped<IFlightRepository, FlightRepository>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IPassengerRepository, PassengerRepository>();

            // ── Infrastructure: flight providers ──────────────────────────────────────
            builder.Services.AddScoped<IFlightProvider, GlobalAirProvider>();
            builder.Services.AddScoped<IFlightProvider, BudgetWingsProvider>();

            // ── Application services ──────────────────────────────────────────────────
            builder.Services.AddScoped<IFlightService, FlightService>();

            // ── Provider settings from appsettings.json ───────────────────────────────
            builder.Services.Configure<FlightProviderSettings>("GlobalAir", builder.Configuration.GetSection("FlightProviders:GlobalAir"));
            builder.Services.Configure<FlightProviderSettings>("BudgetWings", builder.Configuration.GetSection("FlightProviders:BudgetWings"));

            // ─────────────────────────────────────────────────────────────────────────

            var app = builder.Build();

            app.UseExceptionHandler();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "SkyRoute API"));
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }
            app.UseCors();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
