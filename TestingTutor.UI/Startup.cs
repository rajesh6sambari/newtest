using System.Linq;
using System.Security.Claims;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.Data;

using TestingTutor.UI.Data.ViewModels;
using TestingTutor.UI.Hubs;
using TestingTutor.UI.Options;
using TestingTutor.UI.Security;
using TestingTutor.UI.Services;
using TestingTutor.UI.Utilities;

namespace TestingTutor.UI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddTransient<ApplicationDbContext>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<ApplicationDbContext>();
            services.AddHangfire(x => x.UseSqlServerStorage("Server=DESKTOP-8652KA7;Database=aspnet-TestingTutor-HangfireDb;Trusted_Connection=True;MultipleActiveResultSets=true"));
            
            //services.AddTransient<IAppDbContextFactory, AppDbContextFactory>();
            //services.AddSingleton<EngineSubmitter>();
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Stores.MaxLengthForKeys = 128;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration.GetSection("SendGrid"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc();
            services.AddAuthorization(options =>
                {
                    options.AddPolicy("StudentAndHigherPolicy", p =>
                    {
                        p.RequireAssertion(context =>
                        {
                            return context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value.Equals("Student"))
                                || context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value.Equals("Instructor"))
                                || context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value.Equals("Admin"))
                                || context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value.Equals("SuperAdmin"));
                        });
                    });
                    options.AddPolicy("InstructorAndHigherPolicy", p =>
                    {
                        p.RequireAssertion(context =>
                        {
                            return context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value.Equals("Instructor"))
                                   || context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value.Equals("Admin"))
                                   || context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value.Equals("SuperAdmin"));
                        });
                    });
                    options.AddPolicy("AdminAndHigherPolicy", p =>
                    {
                        p.RequireAssertion(context =>
                        {
                            return context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value.Equals("Admin"))
                                   || context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value.Equals("SuperAdmin"));
                        });
                    });
                    options.AddPolicy("SuperAdminPolicy", p =>
                    {
                        p.RequireAssertion(context =>
                        {
                            return context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value.Equals("SuperAdmin"));
                        });
                    });
                }
            );

            services.AddSignalR();

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, MyUserClaimsPrincipalFactory>();
            services.AddScoped<AssignmentFeedbackViewModel>();

            GlobalConfiguration.Configuration.UseActivator(new EngineSubmitter(services.BuildServiceProvider()));

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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();
            app.UseSignalR(routes =>
            {
                routes.MapHub<SubmissionHub>("/submissionHub");
            });

            app.UseHangfireServer();
            app.UseHangfireDashboard();
        }
    }
}
