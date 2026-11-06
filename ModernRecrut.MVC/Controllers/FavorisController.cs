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

        public FavorisController(IFavorisService favorisService)
        {
            _favorisService = favorisService;
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
                TempData["Success"] = "Offre ajoutée aux favoris.";

            return RedirectToAction("Index", "Emplois");


        }



        // POST: FavorisController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SupprimerFavori(int emploiId)
        {
            bool resultat = await _favorisService.Supprimer(emploiId);

            TempData["Message"] = resultat
            ? "Favori supprimé."
            : "Impossible de supprimer ce favori.";

            return RedirectToAction(nameof(Index));
          
        }
    }
}
