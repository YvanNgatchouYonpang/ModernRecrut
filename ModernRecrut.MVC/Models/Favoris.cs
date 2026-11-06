namespace ModernRecrut.MVC.Models
{
    public class Favoris
    {
        public string Cle { get; set; } = string.Empty;

        public List<Emplois> ListeEmplois { get; set; } = new();
    }
}
