using Girteka_task.data.entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Configuration;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace Girteka_task.data
{
    public class Context:DbContext
    {
        public DbSet<NetworkObjectData> Objects { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var converter = new EnumToStringConverter<obj_type>();

            var enumConverter = new EnumToStringConverter<obj_type>();
            modelBuilder.Entity<NetworkObjectData>()
                .Property(e => e.Object_Type)
                .HasConversion(enumConverter);

            var enumGVConverter = new EnumToStringConverter<obj_gv_type>();
            modelBuilder.Entity<NetworkObjectData>()
                .Property(e => e.Object_GV_Type)
                .HasConversion(enumGVConverter);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=ElectricalDataDB");
            }
        }
    }
}
