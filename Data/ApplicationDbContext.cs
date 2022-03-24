using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyPersonalBlog.Models;

namespace MyPersonalBlog.Data;
public class ApplicationDbContext : IdentityDbContext<BlogUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
