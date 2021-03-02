using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMSWeb.BusinessServices.Services;
using EMSWeb.BusinessServices.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySqlConnector;

namespace EMSWeb
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

            services.AddControllersWithViews(config => {
                config.Filters.Add<SidebarActionFilter>();
            }).AddRazorRuntimeCompilation().AddSessionStateTempDataProvider();
            services.AddSession();
            services.AddTransient<MySqlConnection>(_ => new MySqlConnection(Configuration["ConnectionStrings:DefaultConnection"]));
            services.AddScoped<IPhrasebookService, PhrasebookService>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IResourceLibService, ResourceLibService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<ITeacherSupportDocumentService, TeacherSupportDocumentService>();
            services.AddScoped<IKnowledgeService, KnowledgeSharedService>();
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

            app.UseRouting();
            app.UseSession();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Index}/{action=Index}/{id?}");
            });
        }
    }
}
