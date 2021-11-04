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
            const string connectionString =
                @"server=localhost;user=root;password=my-secret-pw;database=QapitaIdentityDb";
            //@"Data Source=localhost;database=QapitaIdentityDb;User Id=SA;Password=Test123!";
            var migrationAssemblyName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            Action<DbContextOptionsBuilder> dbCtxOptBuilder = builder =>
                //builder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationAssemblyName));
                builder.UseMySql(connectionString, new MySqlServerVersion("8.0.27-1debian10"),
                    mysql => mysql.MigrationsAssembly(migrationAssemblyName));

            services.AddIdentityServer()
                .AddConfigurationStore(o =>
                {
                    o.ConfigureDbContext = dbCtxOptBuilder;
                })
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