using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IdentityServer4;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityServerWithAspNetIdentity
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //Add UseIdentityServer IdentityServer to the DI system
            //AddDeveloperIdentityServer is a convenient way to quickly setup IdentityServer with in-memory keys and data stores. 
            //This is only useful for development and test scenarios.
            services.AddDeveloperIdentityServer()
            //Configure identity server with in-memory stores, keys, clients and scopes
                .AddInMemoryScopes(Config.GetScopes())
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryUsers(Config.GetUsers());
            ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Debug);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Add UseIdentityServer Middleware to the HTTP pipeline
            app.UseIdentityServer();

            // Add cookie middleware ( used to temporarily store the outcome of the external authentication in a temporary cookie)
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,

                AutomaticAuthenticate = false,
                AutomaticChallenge = false
            });

            // Add Google middleware
            app.UseGoogleAuthentication(new GoogleOptions
            {
                AuthenticationScheme = "Google",
                DisplayName = "Google",
                SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,

                // Tutorial Config
                //ClientId = "434483408261-55tc8n0cs4ff1fe21ea8df2o443v2iuc.apps.googleusercontent.com";
                //ClientSecret = "3gcoTrEDPPJ0ukn_aYYT6PWo";
                // Mario
                ClientId = "1028694897664-gn06cqserfckbusa4lu3o76jlmvmhmso.apps.googleusercontent.com",
                ClientSecret = "ovh_8DMUKvD6eUWjUm6an1UF"
            });

            //Add MVC and static files to your pipeline:
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
