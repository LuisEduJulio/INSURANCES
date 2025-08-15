using INSURANCES.CORE.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace INSURANCES.DATA.Factory
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<HiringEntity> HiringEntities { get; set; } = null!;
        public DbSet<ProposalEntity> ProposalEntities { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}