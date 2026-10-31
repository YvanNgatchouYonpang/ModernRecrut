using Microsoft.EntityFrameworkCore;
using ModernRecrut.Emplois.API.Data; 
using ModernRecrut.Emplois.API.Interfaces;
using ModernRecrut.Emplois.API.Models;

namespace ModernRecrut.Emplois.API.Services
{
    public class EmploiService : IEmploiService
    {
         private readonly EmploiDbContext _context;

         public EmploiService(EmploiDbContext context)
        {
            _context = context;
        }

        public Task<Emploi> AjouterAsync(Emploi emploi)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ModifierAsync(int id, Emploi emploi)
        {
            throw new NotImplementedException();
        }

        public async Task<Emploi?> ObtenirParIdAsync(int id)
        {
            return await _context.Emplois.FindAsync(id);
        }

        public async Task<IEnumerable<Emploi>> ObtenirTousAsync(string? intitule)
        {
            var aujourdHui = DateTime.Today;

            IQueryable<Emploi> requete = _context.Emplois.Where(e => e.DateDebutAffichage.Date <= aujourdHui &&
            e.DateFinAffichage.Date >= aujourdHui);

            if(!string.IsNullOrWhiteSpace(intitule))
                {
                requete = requete.Where(e => e.Intitule.Contains(intitule));
            }
            return await requete.ToListAsync();
        }

        public Task<bool> SupprimerAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
