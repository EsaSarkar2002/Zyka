using Microsoft.EntityFrameworkCore;
using Zyka.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ZykaDbContext>(options=>options.UseSqlServer("Server=.;Database=ZykaDB;Trusted_Connection=True;MultipleActiveResultSets=true"));))
var app = builder.Build();

using (var scope = app.Services.CreateScope())

{

    var dbContext = scope.ServiceProvider.GetRequiredService<ZykaDbContext>();

    DbSeeder.Seed(dbContext);

}

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(

    name: "default",

    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

