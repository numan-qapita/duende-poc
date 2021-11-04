using System;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Qapita.IdentityServer.CustomGrants;

namespace Qapita.IdentityServer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureQapitaIdP(this IServiceCollection services,
            IConfiguration configuration, IWebHostEnvironment environment)
        {
            var connectionString = configuration.GetConnectionString("MySqlConnection");
            var migrationAssemblyName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            var mySqlVersion = ServerVersion.AutoDetect(connectionString);

            Action<DbContextOptionsBuilder> dbCtxOptBuilder = builder =>
                builder.UseMySql(connectionString, mySqlVersion,
                    mysql => mysql.MigrationsAssembly(migrationAssemblyName));

            services.AddIdentityServer()
                .AddConfigurationStore(o => { o.ConfigureDbContext = dbCtxOptBuilder; })
                .AddOperationalStore(o =>
                {
                    o.ConfigureDbContext = dbCtxOptBuilder;
                    o.EnableTokenCleanup = true;
                    o.TokenCleanupInterval = 3600;
                })
                .AddExtensionGrantValidator<QapitaTenantGrantValidator>();
            return services;
        }
    }
}