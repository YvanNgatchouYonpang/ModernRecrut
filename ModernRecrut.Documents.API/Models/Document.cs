namespace ModernRecrut.Documents.API.Models
{
    public class Document
    {
        public int IdUtilisateur { get; set; }
        public string TypeDocument { get; set; } = string.Empty;
        public IFormFile Fichier { get; set; } = null!;


    }
}
