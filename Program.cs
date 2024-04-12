//using CarRentalApp.Data;
//using CarRentalApp.Repositories;
//using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
//builder.Services.AddDbContext<LMSDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultString")));

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
