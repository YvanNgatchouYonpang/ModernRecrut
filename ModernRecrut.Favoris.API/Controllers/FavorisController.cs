using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModernRecrut.Favoris.API.Models;
using ModernRecrut.Favoris.API.Services;

namespace ModernRecrut.Favoris.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavorisController : ControllerBase
    {
        private readonly FavorisService _favorisService;

        public FavorisController(FavorisService favorisService)
        {
            _favorisService = favorisService;
        }

        // Récupérer l'adresse IP du client
        private string? ObtenirAdresseIP()
        {
            return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
        }

        // GET : api/Favoris
        [HttpGet]
        public IActionResult GetFavoris()
        {
            string? adresseIP = ObtenirAdresseIP();

            if (adresseIP == null)
                return BadRequest("Impossible de déterminer l'adresse IP.");

            Favori favori = _favorisService.ObtenirFavoris(adresseIP);

            return Ok(favori);
        }

        // POST : api/Favoris
        [HttpPost]
        public async Task<IActionResult> AjouterFavori(
            [FromBody] FavorisRequete request)
        {
            string? adresseIP = ObtenirAdresseIP();

            if (adresseIP == null)
                return BadRequest("Impossible de déterminer l'adresse IP.");

            bool resultat = await _favorisService.AjouterFavori(
                adresseIP,
                request.OffreId
            );

            if (!resultat)
                return BadRequest(
                    "L'offre n'existe pas ou figure déjà dans les favoris."
                );

            return Ok("Offre ajoutée aux favoris.");
        }


        // DELETE : api/Favoris/1
        [HttpDelete("{emploiId}")]
        public IActionResult SupprimerFavori(int emploiId)
        {
            string? adresseIP = ObtenirAdresseIP();

            if (adresseIP == null)
                return BadRequest("Impossible de déterminer l'adresse IP.");

            bool resultat = _favorisService.SupprimerFavori(
                adresseIP,
                emploiId
            );

            if (!resultat)
                return NotFound("Cette offre n'existe pas dans vos favoris.");

            return Ok("Favori supprimé avec succès.");
        }
    }
}
