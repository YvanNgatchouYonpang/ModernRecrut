using Microsoft.EntityFrameworkCore;
using ModernRecrut.Emplois.API.Models;

namespace ModernRecrut.Emplois.API.Data
{
    public class EmploiDbContext : DbContext
    {
        public EmploiDbContext(DbContextOptions<EmploiDbContext> options) : base(options)
        {
        }

        public DbSet<Emploi> Emplois { get; set; }
    }
}
