using AutoMapper;
using EldorAnnualLeave.Core.Repositories;
using EldorAnnualLeave.Core.Services;
using EldorAnnualLeave.Core.UnitOfWorks;
using EldorAnnualLeave.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EldorAnnualLeave.Web.Filters;
using EldorAnnualLeave.Service.Services;
using EldorAnnualLeave.Data.Repositories;
using EldorAnnualLeave.Data.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using EldorAnnualLeave.Core.Models;

namespace EldorAnnualLeave.Web
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
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<NotFoundFilter>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IService<>), typeof(Service.Services.Service<>));

            services.AddScoped<IAnnualLeaveTypeService, AnnualLeaveTypeService>();
            services.AddScoped<ICalendarService, CalendarService>();
            services.AddScoped<IAnnualLeaveIncreaseService, AnnualLeaveIncreaseService>();
            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IAppRoleService, AppRoleService>();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:SqlConStr"].ToString(), o =>
                {
                    o.MigrationsAssembly("EldorAnnualLeave.Data");
                });
            });

            services.AddDbContext<ApplicationIdentityDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:SqlConStr"].ToString(), o =>
                {
                    o.MigrationsAssembly("EldorAnnualLeave.Data");
                });
            });

            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();

            //services.AddMvcCore().AddAuthorization();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));

                options.AddPolicy("RequireManagerRole", policy => policy.RequireRole("Manager"));

                options.AddPolicy("RequireMemberRole", policy => policy.RequireRole("Member"));
            });


            services.Configure<IdentityOptions>(options =>
            {
                // password

                //options.Password.RequireDigit = true;
                //options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;
                //options.Password.RequireNonAlphanumeric = true;
                //options.Password.RequireUppercase = true;

                //options.Lockout.MaxFailedAccessAttempts = 5;
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                // options.User.AllowedUserNameCharacters = "";
                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            CookieBuilder cookieBuilder = new CookieBuilder();

            cookieBuilder.Name = "Eldor";
            cookieBuilder.HttpOnly = false;
            cookieBuilder.SameSite = SameSiteMode.Lax;
            cookieBuilder.SecurePolicy = CookieSecurePolicy.SameAsRequest;

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Home/LoginPage");
                //options.LogoutPath = new PathString("/Member/Logout");
                options.Cookie = cookieBuilder;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = System.TimeSpan.FromDays(60);
                options.AccessDeniedPath = new PathString("/Home/AccessDenied");
            });

            services.AddControllersWithViews();
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

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=LoginPage}");

            });
        }
    }
}