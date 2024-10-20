using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure
{
    public class GlobalMWContextFactory : IDesignTimeDbContextFactory<GlobalMWContext>
    {
        public GlobalMWContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GlobalMWContext>();
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=master;User Id=sa;Password=!Tentacion123;TrustServerCertificate=true;");

            return new GlobalMWContext(optionsBuilder.Options);
        }
    }
}