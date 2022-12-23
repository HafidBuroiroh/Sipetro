using Login.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace Login.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }


        public DbSet<Master_Data> Master_Data { get; set; }
        public DbSet<Saved_Data> Saved_Data { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Master_Data>().ToTable("master_data");
            modelBuilder.Entity<Saved_Data>().ToTable("saved_data");
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var insertedEntries = this.ChangeTracker.Entries()
                                   .Where(x => x.State == EntityState.Added)
                                   .Select(x => x.Entity);

            foreach (var insertedEntry in insertedEntries)
            {
                var auditableEntity = insertedEntry as Master_Data;
                //If the inserted object is an . 
                if (auditableEntity != null)
                {
                    auditableEntity.DateCreated = DateTimeOffset.UtcNow;
                }
            }

            var modifiedEntries = this.ChangeTracker.Entries()
                       .Where(x => x.State == EntityState.Modified)
                       .Select(x => x.Entity);

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
