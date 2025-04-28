using Bank2.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//string localIp = "192.168.29.212";
//int port = 7251;
//Hosting
//builder.WebHost.UseUrls("https://192.168.29.212");
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BankContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
options.UseSqlite(builder.Configuration.GetConnectionString("SqliteBankDatabase")));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=LoginPage}/{id?}")
    //pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
