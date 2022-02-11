using Microsoft.EntityFrameworkCore;
using lab1.Models;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace lab1.Storage
{
    public class TMSContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=NTR21Z;user=NTR;password=Zx5Ec#5");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public TMSContext(DbContextOptions<TMSContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ActivityEntry> ActivityEntries { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<MonthInfo> MonthInfos { get; set; }
    }
}