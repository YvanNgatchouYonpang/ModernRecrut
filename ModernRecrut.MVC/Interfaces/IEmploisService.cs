using ModernRecrut.MVC.Models;

namespace ModernRecrut.MVC.Interfaces
{
    public interface IEmploisService
    {
        Task<bool> Ajouter(Emplois emplois);
        Task<Emplois> ObtenirSelonId(int id);
        Task<bool> Modifier(Emplois emplois);
        Task<bool> Supprimer(Emplois emplois);
        Task<IEnumerable<Emplois>> ObtenirTout();
    }
}
