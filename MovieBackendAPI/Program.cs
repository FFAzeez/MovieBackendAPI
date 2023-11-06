using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MovieBackendAPI.Business.Extensions;
using MovieBackendAPI.Infrastructure.Persistence.Context;
using MovieBackendAPI.Middleware;
using Prometheus;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.WebHost.ConfigureAppConfiguration((hostingContext, config) =>
{
    var env = hostingContext.HostingEnvironment;
    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
   .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
    config.AddEnvironmentVariables(prefix: "OMNI_");
});

builder.Logging.AddApplicationInsights();
builder.Logging.AddConsole();
builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft.*", LogLevel.Error);
builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("OpenSleigh.*", LogLevel.Error);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// ConfigureCaching
if (builder.Environment.IsDevelopment() || builder.Environment.IsEnvironment("local"))
{
    builder.Services.AddDistributedMemoryCache();
}
else
{
    var cacheConnection = builder.Configuration["CacheConnection"];
    cacheConnection = cacheConnection.Replace("{CacheName}", builder.Configuration["CACHE_HOST_NAME"]);
    cacheConnection = cacheConnection.Replace("{AccessKey}", builder.Configuration["ACCESS_KEY"]);
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = cacheConnection;
    });
}
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
    builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    );


});

    builder.Services.AddDatabaseService<AppDbContext>(builder.Configuration).AddUnitOfWork<AppDbContext>();
builder.Services.AddServices(builder.Configuration);
builder.Services.AddApplicationInsightsTelemetry();


// Add Swagger 
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MovieBackendAPI-Service", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme, 
                                Id = "Bearer"
                            }
                        },
                        new string[] {}

                    }
                });
});

var counter = Metrics.CreateCounter("movie_path_counter", "Counts requests to the omni scheduler API endpoints", new CounterConfiguration
{
    LabelNames = new[] { "method", "endpoint" }
});

var app = builder.Build();
app.Use((context, next) =>
{
    counter.WithLabels(context.Request.Method, context.Request.Path).Inc(); return next();
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("local"))
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseAuthentication();
app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
