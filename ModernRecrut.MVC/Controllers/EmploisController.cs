using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using System.Diagnostics;

namespace ModernRecrut.MVC.Controllers
{
    public class EmploisController : Controller
    {
        private readonly IEmploisService _emploisService;
        private readonly IFavorisService _favorisService;

        private readonly ILogger<EmploisController> _logger;

        public EmploisController(IEmploisService emploisService, IFavorisService favorisService, ILogger<EmploisController> logger)
        {
            _emploisService = emploisService;
            _favorisService = favorisService;
            _logger = logger;
        }

        // GET: EmploisController
        public async Task<IActionResult> Index(string searchString)
        {
            var emplois = await _emploisService.ObtenirTout();

            if (!string.IsNullOrEmpty(searchString))
            {
                emplois = emplois.Where(e => e.Intitule.ToLower().Contains(searchString.ToLower()));
            }
            return View(emplois);
        }

        // GET: EmploisController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var emplois = await _emploisService.ObtenirSelonId(id);
            return View(emplois);
        }


        // GET: EmploisController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmploisController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Intitule,DateDebutAffichage,DateFinAffichage,Description")] Emplois emplois)
        {
            bool resultat = await _emploisService.Ajouter(emplois);


            if (resultat)
            {
                TempData["Message"] = "Offre d'emploi ajoutée.";

                _logger.LogInformation(
                    "L'ajout de l'offre d'emploi {Intitule} réussie.",
                    emplois.Intitule
                );
            }
            else
            {
                TempData["Message"] = "Impossible d'ajouter l'offre.";

                _logger.LogWarning(
                    "Échec de l'ajout de l'offre d'emploi ."
                );
            }


            return RedirectToAction(nameof(Index));
        }



        // GET: EmploisController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var emplois = await _emploisService.ObtenirSelonId(id);
            return View(emplois);
        }

        // POST: EmploisController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Intitule,DateDebutAffichage,DateFinAffichage,Description")] Emplois emplois,int id)
        {
            bool resultat = await _emploisService.Modifier(emplois);

            if (resultat)
            {
                TempData["Message"] = "Offre d'emploi modifiée.";

                _logger.LogInformation(
                    "Modification de l'offre d'emploi {Id} réussie.",
                    emplois.Id
                );
            }
            else
            {
                TempData["Message"] = "Impossible de modifier l'offre.";

                _logger.LogWarning(
                    "Échec de la modification de l'offre d'emploi {Id}.",
                    emplois.Id
                );
            }
            return RedirectToAction(nameof(Index));
        }


        // GET: EmploisController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var emplois = await _emploisService.ObtenirSelonId(id);
            return View(emplois);
        }

        // POST: EmploisController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Emplois emplois)
        {
            bool resultat = await _emploisService.Supprimer(emplois);

            if (resultat)
            {
                TempData["Message"] = "Offre d'emploi supprimée.";

                _logger.LogInformation(
                    "Suppression de l'offre d'emploi {Id} réussie.",
                    emplois.Id
                );
            }
            else
            {
                TempData["Message"] = "Impossible de supprimer l'offre.";

                _logger.LogWarning(
                    "Échec de la suppression de l'offre d'emploi {Id}.",
                    emplois.Id
                );
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
