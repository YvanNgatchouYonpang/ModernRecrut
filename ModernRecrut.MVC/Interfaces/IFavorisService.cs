using ModernRecrut.MVC.Models;

namespace ModernRecrut.MVC.Interfaces
{
    public interface IFavorisService
    {
         Task<bool> Ajouter(int offreId);
         Task<bool> Supprimer(int idEmploi);
         Task<Favoris?> ObtenirTout();
    }
}
