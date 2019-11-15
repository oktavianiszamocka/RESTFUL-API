using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTAPI.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions opt) : base(opt)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<Note>().Property<DateTime>("CreatedDate");
            modelbuilder.Entity<Note>().Property<DateTime>("ModifiedDate");


        }


        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Note && e.State == EntityState.Added ||
                e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                entityEntry.Property("ModifiedDate").CurrentValue = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property("CreatedDate").CurrentValue = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }

        public DbSet<Note> Notes { get; set; }

        public DbSet<NoteVersion> NotesVersions { get; set; }
    }
}
