using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyPersonalBlog.Data;
using MyPersonalBlog.Models;
using MyPersonalBlog.Services;
using MyPersonalBlog.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(ConnectionService.GetConnectionString(builder.Configuration)));
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<BlogUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultUI()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<DataService>();
builder.Services.AddScoped<BlogSearchService>();
builder.Services.AddScoped<IBlogEmailSender, EmailService>();
builder.Services.AddScoped<IImageService, BasicImageService>();
builder.Services.AddScoped<ISlugService, BasicSlugService>();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

var app = builder.Build();

var dataService = app.Services
                     .CreateScope()
                     .ServiceProvider
                     .GetRequiredService<DataService>();

await dataService.ManageDataAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
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

app.MapControllerRoute(
    name: "SlugRoute",
    pattern: "Posts/UrlFriendly/{slug}",
    defaults: new { controller = "Posts", action = "Details"}
    );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
