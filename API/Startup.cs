using BUSINESS.Abstract;
using BUSINESS.Concrete;
using ENTITIES.Entities;
using FormHelper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using REPOSITORIES.Abstract;
using REPOSITORIES.Concrete;
using REPOSITORIES.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
            services.AddControllersWithViews();
            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddFormHelper();
            services.AddDbContext<BlogDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("Blog")));
            services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<BlogDbContext>().AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);
            services.AddTransient(typeof(IGenericService<>), typeof(GenericManager<>));
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            //{
            //    options.AccessDeniedPath = "/Home/Eriþim Engellendi";       //ÝF you are dont have permission u will redirect this page
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);       //Cookie timer
            //    options.SlidingExpiration = true;       //cookie sliding

            //    options.LoginPath = "/Home/Login";
            //    options.Events.OnRedirectToLogin = context =>
            //    {
            //        context.Response.Redirect(context.RedirectUri);
            //        return Task.CompletedTask;
            //    };

            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
