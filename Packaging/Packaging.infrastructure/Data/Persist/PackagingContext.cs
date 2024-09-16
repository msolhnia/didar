using Microsoft.EntityFrameworkCore;
using Packaging.domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Packaging.infrastructure.Data.Persist
{
    public class PackagingContext : DbContext
    {
        public DbSet<ModuleModel> TTMS { get; set; }

        public PackagingContext(DbContextOptions<PackagingContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
