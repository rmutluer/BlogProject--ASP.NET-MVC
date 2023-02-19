using AutoMapper;
using BUSINESS.Abstract;
using BUSINESS.Concrete;
using ENTITIES.Entities;
using FluentValidation.AspNetCore;
using FormHelper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using REPOSITORIES.Abstract;
using REPOSITORIES.Concrete;
using REPOSITORIES.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI
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
         
            services.AddAutoMapper();
            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddFormHelper().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>()); 
            services.AddDbContext<BlogDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("Blog")));
            services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<BlogDbContext>().AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);
            services.AddTransient(typeof(IGenericService<>), typeof(GenericManager<>));
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddDistributedMemoryCache();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Login/AccessDeniedPage");
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);       //cookie timer
                options.SlidingExpiration = true;       //cookie sliding

                options.LoginPath = new PathString("/Login/Login");
                //options.Events.OnRedirectToLogin = context =>
                //{
                //    context.Response.Redirect(context.RedirectUri);
                //    return Task.CompletedTask;
                //};
            });





        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseFormHelper();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=HomePage}/{action=Index}/{id?}");
            });
        }
    }
}
