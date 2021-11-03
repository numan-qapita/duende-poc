using System;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Qapita.IdentityServer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureQapitaIdP(this IServiceCollection services,
            IConfiguration configuration, IWebHostEnvironment environment)
        {
            const string connectionString =
                @"Data Source=localhost;database=QapitaIdentityDb;User Id=SA;Password=Test123!";
            var migrationAssemblyName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            Action<DbContextOptionsBuilder> dbCtxOptBuilder = builder =>
                builder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationAssemblyName));

            services.AddIdentityServer()
                .AddConfigurationStore(o => o.ConfigureDbContext = dbCtxOptBuilder)
                .AddOperationalStore(o =>
                {
                    o.ConfigureDbContext = dbCtxOptBuilder;
                    o.EnableTokenCleanup = true;
                    o.TokenCleanupInterval = 3600;
                });
            return services;
        }
    }
}