namespace ModernRecrut.Emplois.API.Models
{
    public class Emploi
    {
        public int Id { get; set; }
        public string? Intitule { get; set; }

        public DateTime DateDebutAffichage { get; set; }
        public DateTime DateFinAffichage { get; set; }

        public string? Description { get; set; }
    }
}
