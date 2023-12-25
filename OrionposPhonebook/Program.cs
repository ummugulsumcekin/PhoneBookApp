using Microsoft.EntityFrameworkCore;
using OrionposPhonebook.Models.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrionposPhonebookContext>(options => options.UseSqlite("Data Source=.\\orionposphonebook.db;"));

builder.Services.AddHttpContextAccessor();

builder.Services.AddSession();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=Index}/{id?}");

app.Run();
