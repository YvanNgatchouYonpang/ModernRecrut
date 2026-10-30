using System.ComponentModel.DataAnnotations;

namespace ModernRecrut.Emplois.API.Models
{
    public class Emploi
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "L'intitulé du poste est obligatoire.")]
        public string Intitule { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date de début d'affichage est obligatoire.")]
        public DateTime DateDebutAffichage { get; set; }

        [Required(ErrorMessage = "La date de fin d'affichage est obligatoire.")]
        public DateTime DateFinAffichage { get; set; }

        [Required(ErrorMessage = "La description du poste est obligatoire.")]

        public string Description { get; set; } = string.Empty;
    }
}
