using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using DataAccess;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            // Get GitHub Oauth secrets
            Dictionary<string, string> dict = GetSecretsFromParameterStore("/DevProj").Result;
            string GitHubOAuthClientId = dict["GitHubOAuthClientId"];
            string GitHubOAuthClientSecret = dict["GitHubOAuthClientSecret"];

            // Add DynamoDB user store
            services.AddSingleton<DynamoDBDataAccessLayer>(x => new DynamoDBDataAccessLayer(new Amazon.DynamoDBv2.AmazonDynamoDBClient()));
            services.AddDefaultIdentity<DynamoDBUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddUserStore<DynamoDBUserStore>();

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

        /// <summary>
        /// Function to get the app secrets from the AWS systems manager Parameter Store.
        /// </summary>
        /// <param name="Path">The Parameter Store path/prefix</param>
        /// <returns>
        /// Dictionary of all parameters under that prefix with the item's names (minus the path prefix) as the 
        /// key and the decrypted value as the value.
        /// </returns>
        private async Task<Dictionary<string, string>> GetSecretsFromParameterStore(string Path)
        {
            using (var client = new AmazonSimpleSystemsManagementClient(Amazon.RegionEndpoint.EUWest2))
            {
                var result = await client.GetParametersByPathAsync(new GetParametersByPathRequest
                {
                    Path = Path,
                    Recursive = true,
                    WithDecryption = true
                });
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                result.Parameters.ForEach(item =>
                {
                    parameters.Add(item.Name.Replace(Path + "/", ""), item.Value);
                });
                return parameters;
            }
        }
    }
}
