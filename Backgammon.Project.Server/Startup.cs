using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backgammon.Project.DAL;
using Backgammon.Project.DAL.DataContext;
using Backgammon.Project.Server.Hubs;
using Backgammon.Project.Server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backgammon.Project.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddDbContext<BgDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("BgDb")));
            services.AddScoped<UnitOfWork>();
            services.AddSingleton<SessionDataService>();
            services.AddSingleton<GameRoomsService>();
            services.AddScoped<UserService>();
            services.AddScoped<UserHubNotificationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<UsersHub>(new PathString("/Users"));
            });
        }
    }
}
