using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace inventory_control_of_dep_dal.Domain
{
    public class DBContext : IdentityDbContext<User>
    {
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Faculty> Faculty { get; set; }
        public virtual DbSet<MaterialValue> MaterialValue { get; set; }
        public virtual DbSet<OperationsType> OperationsType { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<InventoryBook> InventoryBook { get; set; }
        public virtual DbSet<Aprovar> Aprovar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("InventoryDB");
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("AspNetUsers");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<Faculty>().ToTable("Faculty");
            modelBuilder.Entity<MaterialValue>().ToTable("MaterialValue");
            modelBuilder.Entity<OperationsType>().ToTable("OperationsType");
            modelBuilder.Entity<Position>().ToTable("Position");
            modelBuilder.Entity<Room>().ToTable("Room");
            modelBuilder.Entity<InventoryBook>().ToTable("InventoryBook");
            modelBuilder.Entity<Aprovar>().ToTable("Aprovar");
        }
    }
}
