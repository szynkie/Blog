using Blog.Data;
using Blog.Data.FileManager;
using Blog.Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(_config["DefaultConnection"]));
            services.AddIdentity<IdentityUser, IdentityRole>(options => { 
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
            })
                //.AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
            });

            services.AddTransient<IRepository, Repository>();
            services.AddTransient<IFileManager, FileManager>();

            services.AddMvc(option => option.EnableEndpointRouting = false);
        }
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();

           
        }

    } }
