using System.ComponentModel.DataAnnotations;

namespace ModernRecrut.MVC.Models
{
    public class Emplois
    {
        [Display(Name = "Identifiant")]
        public int Id { get; set; }

        [Required(ErrorMessage = "L'intitulé du poste est obligatoire.")]
        [Display(Name = "Intitulé")]
        public string Intitule { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date de début d'affichage est obligatoire.")]
        [Display(Name = "Date de début d'affichage")]
        public DateTime DateDebutAffichage { get; set; }

        [Required(ErrorMessage = "La date de fin d'affichage est obligatoire.")]
        [Display(Name = "Date de fin d'affichage")]
        public DateTime DateFinAffichage { get; set; }

        [Required(ErrorMessage = "La description du poste est obligatoire.")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;
    }
}
