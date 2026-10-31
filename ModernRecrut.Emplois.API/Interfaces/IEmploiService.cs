using ModernRecrut.Emplois.API.Models;

namespace ModernRecrut.Emplois.API.Interfaces
{
    public interface IEmploiService
    {
        Task<IEnumerable<Emploi>> ObtenirTousAsync(string? intitule);
        Task<Emploi> ObtenirParIdAsync(int id); 
        Task<Emploi> AjouterAsync(Emploi emploi);
        Task<bool> ModifierAsync(int id, Emploi emploi);

        Task<bool> SupprimerAsync(int id);
    }
}
