
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieBackendAPI.Business.Mapper;
using MovieBackendAPI.Domain.Utility;
using MovieBackendAPI.Infrastructure.Persistence.UnitOfWork;
using System;

namespace MovieBackendAPI.Business.Extensions
{
    public static class ServiceCollectionsExtension
    {
        public static IServiceCollection AddDatabaseService<TContext>(this IServiceCollection service, IConfiguration configuration)
          where TContext : DbContext
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            service.AddDbContext<TContext>(options =>
           {
               options.UseSqlServer(connectionString, c => c.CommandTimeout(120));
           });

            return service;
        }

        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services)
         where TContext : DbContext
        {
            services.AddScoped<IRepositoryFactory, UnitOfWork<TContext>>();
            services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
            services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();
            return services;
        }


        public static IServiceCollection AddServices(this IServiceCollection service, IConfiguration configuration)
        {
            var assembly = AppDomain.CurrentDomain.Load("MovieBackendAPI.Business");
            service.AddMediatR(_=>_.RegisterServicesFromAssemblies(assembly));
            service.AddTransient<ICacheService, CacheService>();
            service.AddScoped<IFileUpload, FileUpload>();
            service.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            return service;
        }

    }
}
