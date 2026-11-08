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
        private readonly ILogger<FavorisController> _logger;

        public FavorisController(FavorisService favorisService,ILogger<FavorisController>logger)
        {
            _favorisService = favorisService;
            _logger = logger;
        }

        // Récupérer l'adresse IP du client
        private string? ObtenirAdresseIP()
        {
            return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
        }


        /// <summary>
        /// Retourne les favoris de l'utilisateur.
        /// </summary>
        /// <response code="200">favoris trouvés et retournés</response>
        /// <response code="404">favoris introuvables</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
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

        /// <summary>
        /// Ajoute une offre d'emploi aux favoris de l'utilisateur.
        /// </summary>
        /// <remarks>
        /// Je manque d'imagination
        /// 
        /// Exemple d'utilisation
        /// 
        /// POST / Favoris
        /// {
        ///     "offreId": 2
        /// }
        /// 
        /// </remarks>
        /// <param name="id">id de l'offre à ajouter aux favoris</param>
        /// <response code="201">offre ajoutée aux favoris</response>
        /// <response code="401">Erreur de validation</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
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

            _logger.LogInformation(
                "Offre d'emploi avec l'ID {OffreId} ajoutée aux favoris pour l'adresse IP {AdresseIP}.",
                request.OffreId,
                adresseIP
            );

            return Ok("Offre ajoutée aux favoris.");
        }


        /// <summary>
        /// Supprime un favori pour l'utilisateur en fonction de son adresse IP.
        /// </summary>
        /// <remarks>Je manque d'imagination</remarks>
        /// <param name="id">id de l'offre à supprimer des favoris</param>
        /// <response code="204">offre supprimée des favoris</response>
        /// <response code="404">offre introuvable pour l'id spécifié</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>

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

            _logger.LogInformation(
                "Offre d'emploi avec l'ID {OffreId} supprimée des favoris pour l'adresse IP {AdresseIP}.",
                emploiId,
                adresseIP
            );

            return Ok("Favori supprimé avec succès.");
        }
    }
}
