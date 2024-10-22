using MuseWave.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MuseWave.Identity
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlServer("Server=localhost,1433;Database=MWUsers;User Id=sa;Password=!Tentacion123;TrustServerCertificate=true;");
        // }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}