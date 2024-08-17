using ALSETDotnetTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//DEVELOPER CODE
// send the string connection (called devconnection) from appsetitngs jason 
builder.Services.AddDbContext<MainDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));

//configurate cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
//
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

var fileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files"));
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = fileProvider,
    RequestPath = "/Files"
});


app.UseRouting();

// use of configurated cors
app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
