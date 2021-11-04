// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using System.Linq;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Qapita.IdentityServer.Extensions;

namespace Qapita.IdentityServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddTransient<IClaimsService, QapitaClaimsService>();
            // var builder = services.AddIdentityServer(options =>
            // {
            //     // https://docs.duendesoftware.com/identityserver/v5/fundamentals/resources/
            //     options.EmitStaticAudienceClaim = true;
            // })
            //     .AddInMemoryIdentityResources(Config.IdentityResources)
            //     .AddInMemoryApiScopes(Config.ApiScopes)
            //     .AddInMemoryClients(Config.Clients)
            //     .AddExtensionGrantValidator<QapitaTenantGrantValidator>();

            services.ConfigureQapitaIdP(Configuration, Environment);
        }

        public void Configure(IApplicationBuilder app)
        {
            InitializeDatabase(app);
            
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
        }
        
        private static void InitializeDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
            if (serviceScope == null) return;
            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();
            if (!context.Clients.Any())
            {
                foreach (var client in Config.Clients)
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.IdentityResources)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in Config.ApiScopes)
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
        }
    }
}
