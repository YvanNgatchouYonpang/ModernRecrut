
using ModernRecrut.Favoris.API.DTOs;

namespace ModernRecrut.Favoris.API.Models
{
    public class Favori
    {
        public string Cle { get; set; } = string.Empty;

        public List<EmploiDto> ListeEmplois { get; set; } = new();
    }
}
