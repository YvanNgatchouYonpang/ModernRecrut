using ModernRecrut.Documents.API.Interfaces;
using ModernRecrut.Documents.API.Models;

namespace ModernRecrut.Documents.API.Services
{
    public class DocumentService:IDocumentService
    {
        private readonly IWebHostEnvironment _env;

        public DocumentService(IWebHostEnvironment env)
        {
            _env = env;
        }

        //enregistrer un document
        public async Task<string> EnregistrerDocument(
            Document document)
        {
            //chemin du dossier document
            string documentPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents");
            if(!Directory.Exists(documentPath))
            {
                Directory.CreateDirectory(documentPath);
            }

            int numeroAleatoire = Random.Shared.Next(10000, 100000);

            string extension = Path.GetExtension(document.Fichier.FileName);

            //nom du document
            string typeDoc = document.TypeDocument.Replace(" ", string.Empty);
            string nomfichier = $"{document.IdUtilisateur}_{typeDoc}_{numeroAleatoire}{extension}";

            //nom complet du document
            string filePath = Path.Combine(documentPath, nomfichier);

            //enregistrer le document
            using (var stream=new FileStream(filePath, FileMode.Create))
            {
                await document.Fichier.CopyToAsync(stream);
            }

            //Retourner le nom du document

            return nomfichier;
        }

        //obtenir la liste des documents
        public List<string> ObtenirDocmuments(int idUtilisateur)
        {
            //chemin du dossier document
            string documentPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents");

            // verification de l'existance du dossier documents
            if (!Directory.Exists(documentPath))
            {
                return new List<string>();
            }

            //recuperation des document appartenat a l'utilisateur

            string prefixe = $"{idUtilisateur}_";

            return Directory.GetFiles(documentPath)
                .Select(Path.GetFileName)
                .Where(nom => nom != null && nom.StartsWith(prefixe))
                .ToList()!;
        }
    }
}
