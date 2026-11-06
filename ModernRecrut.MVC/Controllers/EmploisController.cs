using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;

namespace ModernRecrut.MVC.Controllers
{
    public class EmploisController : Controller
    {
        private readonly IEmploisService _emploisService;
        private readonly IFavorisService _favorisService;

        public EmploisController(IEmploisService emploisService, IFavorisService favorisService)
        {
            _emploisService = emploisService;
            _favorisService = favorisService;
        }

        // GET: EmploisController
        public async Task<IActionResult> Index(string searchString)
        {
            var emplois = await _emploisService.ObtenirTout();

            if (!string.IsNullOrEmpty(searchString))
            {
                emplois = emplois.Where(e => e.Intitule.ToLower().Contains(searchString.ToLower())
                                       || e.Description.ToLower().Contains(searchString.ToLower()));
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
          if(ModelState.IsValid)
            {
                await _emploisService.Ajouter(emplois);
                return RedirectToAction(nameof(Index));
            }
            return View(emplois);
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
            
            if(ModelState.IsValid)
            {
                await _emploisService.Modifier(emplois);
                return RedirectToAction(nameof(Index));
            }
            return View(emplois);
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
            
                await _emploisService.Supprimer(emplois);
                return RedirectToAction(nameof(Index));
            
        }

    }
}
