using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EatAndDrink.Models;

namespace EatAndDrink.Data;


public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }
    
    public DbSet<Dish> Dishes { get; set; }
   
    public DbSet<Restorant> Restorants { get; set; }

}
