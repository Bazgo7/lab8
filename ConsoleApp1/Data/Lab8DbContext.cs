using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1
{
    public class Lab8DbContext : DbContext
    {
        public Lab8DbContext(DbContextOptions<Lab8DbContext> options) : base(options)
        {
        }

        public DbSet<TriangleRecord> TriangleRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TriangleRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                
                entity.HasIndex(e => new { e.Side1, e.Side2, e.Side3 })
                      .IsUnique()
                      .HasDatabaseName("IX_TriangleRecords_Sides");
            });
        }
    }
}
