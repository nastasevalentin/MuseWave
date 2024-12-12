using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MuseWave.Domain.Entities;

namespace Infrastructure;

public class GlobalMWContext : DbContext
{
    public DbSet<Album> Albums { get; set; }
    public DbSet<Song> Songs { get; set; }

    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=MWDB;User Id=sa;Password=!Tentacion123;TrustServerCertificate=true;");
    }

    
    public GlobalMWContext(DbContextOptions<GlobalMWContext> options) : base(options)
    {
    }
}