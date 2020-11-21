using DynamoDBUserStore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DevProjWebApp
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
            string GitHubOAuthClientId = "3c789086229a89c60575";
            string GitHubOAuthClientSecret = "17a7d95f7f8a308b4d1d66da0115ffd07e1356c7";

            if (GitHubOAuthClientId == null || GitHubOAuthClientSecret == null)
            {
                throw new Exception("One or both of the GitHubOAuthClientSecret or GitHubOAuthClientId environment variables are missing!");
            }
            services.AddSingleton<InMemoryUserDataAccess>();
            services.AddDefaultIdentity<DynamoDBUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddUserStore<InMemoryUserStore>();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/signin";
                options.LogoutPath = "/signout";
            }).AddGitHub(options =>
            {
                options.ClientId = GitHubOAuthClientId;
                options.ClientSecret = GitHubOAuthClientSecret;
                options.Scope.Add("user:email");
            });
            services.AddControllersWithViews();
            services.AddRazorPages();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
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
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
