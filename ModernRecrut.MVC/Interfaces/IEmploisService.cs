using ModernRecrut.MVC.Models;

namespace ModernRecrut.MVC.Interfaces
{
    public interface IEmploisService
    {
        Task Ajouter(Emplois emplois);
        Task<Emplois> ObtenirSelonId(int id);
        Task Modifier(Emplois emplois);
        Task Supprimer(Emplois emplois);
        Task<IEnumerable<Emplois>> ObtenirTout();
    }
}
