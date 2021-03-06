using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quicksilver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quicksilver.DAL;
using Quicksilver.DAL.QuicksilverDbContext;
using Microsoft.AspNetCore.Mvc.Authorization;
using Quicksilver.DAL.Interfaces;
using Quicksilver.DAL.Repositories;
using Quicksilver.DAL.IdentityDbContext;
using Quicksilver.BAL.Operations;

namespace Quicksilver
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
            services.AddDbContext<QuicksilverContext>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddMvc().AddRazorRuntimeCompilation().AddMvcOptions(x=>x.Filters.Add(new AuthorizeFilter()));
            DbConnectionString.ConStr = Configuration.GetConnectionString("DefaultConnection");

            //DIs
            services.AddScoped<IAgentRepository, AgentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserOperations, UserOperations>();
            services.AddScoped<IAgentsOperations, AgentOperations>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
