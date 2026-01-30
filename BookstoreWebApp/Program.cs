using BookstoreProjectData;
using BookstoreProjectData.Entities;
using BookstoreWebApp.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookstoreWebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<BookstoreContext>(options =>
                options.UseSqlServer(connectionString));
                                                      builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            //builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
            //    .AddEntityFrameworkStores<BookstoreContext>();

            //builder.Services.AddDbContext<BookstoreContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<BookstoreContext>()
                .AddDefaultTokenProviders().
                AddDefaultUI();
            


            builder.Services.AddControllersWithViews();
           
            builder.Services.AddRazorPages(); 

            //builder.Services.AddDefaultIdentity<User>(options =>
            //{
            //    options.SignIn.RequireConfirmedAccount = false;
            //    options.Password.RequiredLength = 6;
            //}).AddRoles<IdentityRole>()
            //.AddEntityFrameworkStores<BookstoreContext>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var rolemanager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await IdentitySeeder.SeedRolesAsync(rolemanager);
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            
            app.Run();
        }
    }
}
