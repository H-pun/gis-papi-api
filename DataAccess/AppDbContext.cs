using Microsoft.EntityFrameworkCore;
using GISPaPi.Base;
using GISPaPi.DataAccess.Entities;
using GISPaPi.DataAccess.Extensions;

namespace GISPaPi.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var entitiesAssembly = typeof(User).Assembly;
            modelBuilder.RegisterAllEntities<BaseEntity>(entitiesAssembly);
        }
    }
}
