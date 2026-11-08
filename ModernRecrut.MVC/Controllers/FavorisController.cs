using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;

namespace ModernRecrut.MVC.Controllers
{
    public class FavorisController : Controller
    {
        private readonly IFavorisService _favorisService;
        private readonly ILogger<FavorisController> _logger;

        public FavorisController(IFavorisService favorisService,ILogger<FavorisController> logger)
        {
            _favorisService = favorisService;
            _logger = logger;
        }
        // GET: FavorisController
        public async Task<IActionResult> Index()
        {
            var favoris = await _favorisService.ObtenirTout();
            return View(favoris);
        }


        // POST: FavorisController/AjouterFavoris/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AjouterFavoris(int offreId)
        {

            bool resultat = await _favorisService.Ajouter(offreId);


            if (resultat)
            {
                TempData["Message"] = "Offre d'emploi ajoutée aux favoris.";

                _logger.LogInformation(
                    "L'ajout de l'offre d'emploi aux favoris réussie."
                );
            }
            else
            {
                TempData["Message"] = "l'offre existe deja dans les favoris.";

                _logger.LogWarning(
                    "Échec de l'ajout de l'offre d'emploi aux favoris."
                );
            }

            return RedirectToAction("Index", "Emplois");


        }



        // POST: FavorisController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SupprimerFavori(int emploiId)
        {
            bool resultat = await _favorisService.Supprimer(emploiId);

            if (resultat)
            {
                TempData["Message"] = "Favori supprimé.";

                _logger.LogInformation(
                    "Suppression du favori réussie."
                );
            }
            else
            {
                TempData["Message"] = "Impossible de supprimer le favori.";

                _logger.LogWarning(
                    "Échec de la suppression du favori ."
                );
            }

            return RedirectToAction(nameof(Index));
          
        }
    }
}
