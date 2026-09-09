using CleanArchitectureDemo.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace CleanArchitectureDemo.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
