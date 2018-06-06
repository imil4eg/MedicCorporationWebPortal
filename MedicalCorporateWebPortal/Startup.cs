using MedicalCorporateWebPortal.AppData;
using MedicalCorporateWebPortal.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace MedicalCorporateWebPortal
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
            services.AddDbContext<MedicCroporateContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, ApplicationRole>()
                .AddEntityFrameworkStores<MedicCroporateContext>()
                .AddDefaultTokenProviders();

            // Change password policy
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/Security/Login";
                options.SlidingExpiration = true;
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            //using (var scope = scopeFactory.CreateScope())
            //{
            //    var roleService = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            //    var userService = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            //    CreateRoles(roleService, userService).Wait();
            //}
        }

        private async Task CreateRoles(RoleManager<ApplicationRole> roleService, UserManager<User> userService)
        {
            //var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            //var usersManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = roleService;
            var userManager = userService;
            string[] roleNames = { "Администратор", "Пациент", "Врач", "Бухгалтер", "Ресепшен" };
            IdentityResult roleResult;

            foreach(var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new ApplicationRole(roleName));
                }
            }

            var powerUser = new User
            {
                UserName = "Admin",
                Password = "Admin",
                Email = "Admin@mail.ru",
                Gender = Gender.Мужской,
                LastName = "Admin",
                FirstName = "Admin"
            };

            var userPatient = new User
            {
                UserName = "Patient",
                Password = "Patient",
                Email = "Patient@mail.ru",
                Gender = Gender.Мужской,
                LastName = "Patient",
                FirstName = "Patient",
                Role = UserRole.Пациент
            };

            var userDoctor = new User
            {
                UserName = "Doctor",
                Password = "Doctor",
                Email = "Doctor@mail.ru",
                Gender = Gender.Мужской,
                LastName = "Doctor",
                FirstName = "Doctor",
                Role = UserRole.Врач
            };

            var userCalculator = new User
            {
                UserName = "Calcuc",
                Password = "Calcuc",
                Email = "Calcuc@mail.ru",
                Gender = Gender.Мужской,
                LastName = "Calcuc",
                FirstName = "Calcuc",
                Role = UserRole.Бухгалтер
            };


            var _user = await userManager.FindByEmailAsync(powerUser.Email);
            if(_user == null)
            {
                var createPowerUser = await userManager.CreateAsync(powerUser, powerUser.Password);
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(powerUser, "Администратор");
                }
            }

            _user = await userManager.FindByEmailAsync(userPatient.Email);
            if(_user == null)
            {
                var createUserPatient = await userManager.CreateAsync(userPatient, userPatient.Password);
                if (createUserPatient.Succeeded)
                {
                    await userManager.AddToRoleAsync(userPatient, "Пациент");
                }
            }

            _user = await userManager.FindByEmailAsync(userDoctor.Email);
            if(_user == null)
            {
                var createUserDoctor = await userManager.CreateAsync(userDoctor, userDoctor.Password);
                if (createUserDoctor.Succeeded)
                {
                    await userManager.AddToRoleAsync(userDoctor, "Врач");
                }
            }

            _user = await userManager.FindByEmailAsync(userCalculator.Email);
            if(_user == null)
            {
                var createUserCalcuc = await userManager.CreateAsync(userCalculator, userCalculator.Password);
                if (createUserCalcuc.Succeeded)
                {
                    await userManager.AddToRoleAsync(userCalculator, "Бухгалтер");
                }
            }
        }
    }
}
