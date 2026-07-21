using Ferreteria.Core.Models;
using Microsoft.EntityFrameworkCore;
namespace Ferreteria.Data.Context;

public class AppDbContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=ferreteria.db");
        }
    }
}