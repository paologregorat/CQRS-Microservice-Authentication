using Authentication.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Domain.Infrasctructure
{
    public class EntityContext: DbContext
    {
        public EntityContext(DbContextOptions<EntityContext> options) : base(options)
        {
            
        }

        public virtual DbSet<Operatore> Operatori { get; set; } = null!;
    }
    
    public class ApplicationDbContext : DbContext
    {
        private readonly string _connectionString;

        public ApplicationDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }
        
        public virtual DbSet<Operatore> Operatori { get; set; } = null!;
    }
}