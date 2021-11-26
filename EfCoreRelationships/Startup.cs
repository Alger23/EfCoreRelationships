using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EfCoreRelationships.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace EfCoreRelationships
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkNpgsql();
            services.AddDbContext<Models.EfCoreRelationshipsDbContext>(opts =>
                opts.UseNpgsql(Configuration.GetConnectionString("EfCoreRelationships")));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "EfCoreRelationships", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitializeDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EfCoreRelationships v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider
                    .GetRequiredService<EfCoreRelationshipsDbContext>();
#if DEBUG
                // TODO: remove this line, will delete database
                dbContext.Database.EnsureDeleted();
#endif
                if (dbContext.Database.EnsureCreated())
                {
                    var user = new User()
                    {
                        Account = "user",
                        Password = "abc",
                        Created = DateTime.Now,
                    };
                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                }
            }
        }
    }
}