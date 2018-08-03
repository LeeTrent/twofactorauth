using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using lmsextreg.Data;
using lmsextreg.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using lmsextreg.Authorization;

namespace lmsextreg
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
             // Database Connection Parameters
            String connectionString = buildConnectionString();
            
            // WRITE CONNECTION STRING TO THE CONSOLE
            Console.WriteLine("********************************************************************************");
            Console.WriteLine("[Startup] Connection String: " + connectionString);
            Console.WriteLine("********************************************************************************");

            // NOW THAT WE HAVE OUR CONNECTION STRING, WE CAN ESTABLISH OUR DB CONTEXT
            services.AddDbContext<ApplicationDbContext>
            (
                options => options.UseNpgsql(connectionString)
            );

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequiredLength = 12;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true; 
            });
                

            /***************************************************************************************
                
                Require authenticated users
                
                Set the default authentication policy to require users to be authenticated.
                You can opt out of authentication at the Razor Page, controller
                or action method level with the [AllowAnonymous] attribute. 
                
                Setting the default authentication policy to require users to be authenticated
                protects newly added Razor Pages and controllers. 
                
                Having authentication required by default is safer than relying on new controllers
                and Razor Pages to include the [Authorize] attribute.              
            ***************************************************************************************/
            services.AddMvc();
            // requires: using Microsoft.AspNetCore.Authorization;
            //           using Microsoft.AspNetCore.Mvc.Authorization;
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });            
            /***************************************************************** 
                With the requirement of all users authenticated, 
                the AuthorizeFolder and AuthorizePage calls are not required.
            ******************************************************************/
            // services.AddMvc()
            //     .AddRazorPagesOptions(options =>
            //     {
            //         options.Conventions.AuthorizeFolder("/Account/Manage");
            //         options.Conventions.AuthorizePage("/Account/Logout");
            //     });
            /*******************************************************************/
            
            /***************************************************************** 
                Register no-op EmailSender used by account confirmation and password
                reset during development
                For more information on how to enable account confirmation and password reset,
                please visit https://go.microsoft.com/fwlink/?LinkID=532713
            ******************************************************************/
            services.AddSingleton<IEmailSender, EmailSender>();

            // Configure startup to use AuthMessageSenderOptions
            services.Configure<AuthMessageSenderOptions>(Configuration);

            // Register the authorization handlers
            // (not being used at this time)
            // services.AddScoped<IAuthorizationHandler, StudentAuthorizationHandler>();
            // services.AddScoped<IAuthorizationHandler, ApproverAuthorizationHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();
        }

        private String buildConnectionString()
        {
             String connectionString = null;
            try
            {
                connectionString = Environment.GetEnvironmentVariable("LOCAL_CONNECTION_STRING");
                if (connectionString == null)
                {
                    string vcapServices = System.Environment.GetEnvironmentVariable("VCAP_SERVICES");
                    if (vcapServices != null)
                    {
                        dynamic json = JsonConvert.DeserializeObject(vcapServices);
                        foreach (dynamic obj in json.Children())
                        {
                            dynamic credentials = (((JProperty)obj).Value[0] as dynamic).credentials;
                            if (credentials != null)
                            {
                                string host     = credentials.host;
                                string username = credentials.username;
                                string password = credentials.password;
                                string port     = credentials.port;
                                string db_name  = credentials.db_name;

                                connectionString = "Username=" + username + ";"
                                    + "Password=" + password + ";"
                                    + "Host=" + host + ";"
                                    + "Port=" + port + ";"
                                    + "Database=" + db_name + ";Pooling=true;";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in [Startup.buildConnectionString()]:");
                Console.WriteLine(e);
            }
             return connectionString;
        }
    }
}
