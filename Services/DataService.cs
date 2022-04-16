using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyPersonalBlog.Data;
using MyPersonalBlog.Enums;
using MyPersonalBlog.Models;

namespace MyPersonalBlog.Services;

public class DataService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<BlogUser> _userManager;

    public DataService(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<BlogUser> userManager)
    {
        _dbContext = dbContext;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task ManageDataAsync()
    {
        // Create the DB from the Migrations
        await _dbContext.Database.MigrateAsync();
        // Seed a few Roles into the system
        await SeedRolesAync();

        // Seed a few users into the system
        await SeedUsersAsync();
    }


    private async Task SeedRolesAync()
    {
        //If there are already Roles in the system, do nothing.
        if (_dbContext.Roles.Any())
        {
            return;
        }
        foreach(var role in Enum.GetNames(typeof(BlogRole)))
        {
           await _roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    private async Task SeedUsersAsync()
    {
        if (_dbContext.Users.Any())
        {
            return;
        }

        var adminUser = new BlogUser()
        {
            Email = "antwaundoescode@gmail.com",
            UserName = "antwaundoescode@gmail.com",
            FirstName = "Antwaun",
            LastName = "Johnson",
            DisplayName = "AntwaunCodes",
            EmailConfirmed = true
        };

        await _userManager.CreateAsync(adminUser, "Abc&123!");

        await _userManager.AddToRoleAsync(adminUser, BlogRole.Administrator.ToString());
    }     
}
