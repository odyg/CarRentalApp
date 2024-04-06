using CarRentalApp.Data;
//using CarRentalApp.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CarRentalDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultString")));

//builder.Services.AddScoped<BookRepository>();
//builder.Services.AddScoped<ReaderRepository>();
//builder.Services.AddScoped<BorrowingRepository>();

var app = builder.Build();


app.MapControllers();

//app.MapGet("/", () => "Hello World!");

app.Run();
