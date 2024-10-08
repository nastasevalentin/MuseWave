using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MuseWave.Domain.Entities;

namespace Infrastructure;

public class GlobalMWContext : DbContext
{
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Listener> Listeners { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=master;User Id=sa;Password=!Tentacion123;TrustServerCertificate=true;");
    }

    
    public GlobalMWContext(DbContextOptions<GlobalMWContext> options) : base(options)
    {
    }
}