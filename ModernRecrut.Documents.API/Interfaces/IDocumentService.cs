using ModernRecrut.Documents.API.Models;
namespace ModernRecrut.Documents.API.Interfaces
{
    public interface IDocumentService
    {
        Task<string> EnregistrerDocument(Document documents);

        List<string> ObtenirDocmuments(int idUtilisateur);
    }
}
