using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModernRecrut.Emplois.API.Data;
using ModernRecrut.Emplois.API.Models;

namespace ModernRecrut.Emplois.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmploisController : ControllerBase
    {
        private readonly EmploiDbContext _context;
        private readonly ILogger<EmploisController> _logger;
        public EmploisController(EmploiDbContext context, ILogger<EmploisController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retourne la liste des offres d'emploi
        /// </summary>
        /// <response code="200">offres d'emploi trouvées et retournées</response>
        /// <response code="404">offres d'emploi introuvables</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Emploi>>> GetEmplois([FromQuery] string? intitule)
        {
            // Récupérer la date du jour, sans l'heure.
            var aujourdHui = DateTime.Today;

            // Commencer par les offres valides aujourd'hui.
            IQueryable<Emploi> requete = _context.Emplois.Where(e =>e.DateDebutAffichage.Date <= aujourdHui &&
                    e.DateFinAffichage.Date >= aujourdHui);

            // Appliquer le filtre par intitulé s'il est fourni.
            if (!string.IsNullOrWhiteSpace(intitule))
            {
                requete = requete.Where(e =>
                    e.Intitule.Contains(intitule));
            }

            var emplois = await requete.ToListAsync();

            return Ok(emplois);
        }


        /// <summary>
        /// Retourne un l'offre d'emploi spécifique à partir de son id
        /// </summary>
        /// <remarks>Je manque d'imagination</remarks>
        /// <param name="id">id de l'offre d'emploi à retourner</param>
        /// <response code="200">offre d'emploi trouvée et retournée</response>
        /// <response code="404">offre d'emploi introuvable pour l'id spécifié</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Emploi>> GetEmploi(int id)
        {
            var emploi = await _context.Emplois.FindAsync(id);
            if (emploi == null)
            {
                return NotFound();
            }
            return Ok(emploi);
        }

        /// <summary>
        /// Ajoute une nouvelle offre d'emploi dans la base de données.
        /// </summary>
        /// <remarks>
        /// Je manque d'imagination
        /// 
        /// Exemple de requete
        /// 
        /// POST / Emplois
        /// {
        ///      "id": 1,
        ///      "intitule": "Developpeur mobile",
        ///      "dateDebutAffichage": "2026-11-08T10:05:40.162Z",
        ///      "dateFinAffichage": "2026-11-08T10:05:40.162Z",
        ///      "description": "Developpeur .Net Maui, React native"
        /// }
        /// </remarks>
        /// <response code="201">offre d'emploi ajoutée avec succès</response>
        /// <response code="401">erreur de validation</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>

        [HttpPost]
        public async Task<ActionResult<Emploi>> PostEmploi(Emploi emploi)
        {
            if (emploi.DateDebutAffichage == default || emploi.DateFinAffichage == default)
            {
                return BadRequest("Les dates de début et de fin d'affichage sont obligatoires.");
            }

            if (emploi.DateFinAffichage.Date < emploi.DateDebutAffichage.Date)
            {
                return BadRequest("La date de fin d'affichage doit être supérieure ou égale à la date de début.");
            }

            _context.Emplois.Add(emploi);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Création d'une offre d'emploi avec l'Id {emploi.Id}.");

            return CreatedAtAction(
                nameof(GetEmploi),
                new { id = emploi.Id },
                emploi);
        }

        /// <summary>
        /// Modifie une offre d'emploi existante dans la base de données.
        /// </summary>
        /// <remarks>
        /// Je manque d'imagination
        /// Exemple de requete
        /// 
        /// PUT / Emplois
        /// {
        ///      "id": 1,
        ///      "intitule": "Developpeur mobile",
        ///      "dateDebutAffichage": "2026-11-08T10:05:40.162Z",
        ///      "dateFinAffichage": "2026-11-08T10:05:40.162Z",
        ///      "description": "Developpeur .Net Maui, React native"
        /// }
        /// 
        /// </remarks> 
        /// <response code="204">offre d'emploi modifiée avec succès</response>
        /// <response code="401">Erreur de validation</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutEmploi(int id, Emploi emploi)
        {
            if (id != emploi.Id)
            {
                return BadRequest("L'identifiant de l'URL ne correspond pas à celui de l'offre.");
            }

            if (emploi.DateDebutAffichage == default || emploi.DateFinAffichage == default)
            {
                return BadRequest("Les dates de début et de fin d'affichage sont obligatoires.");
            }

            if (emploi.DateFinAffichage.Date < emploi.DateDebutAffichage.Date)
            {
                return BadRequest("La date de fin d'affichage doit être supérieure ou égale à la date de début.");
            }

            var emploiExistant = await _context.Emplois.FindAsync(id);

            if (emploiExistant == null)
            {
                return NotFound();
            }

            emploiExistant.Intitule = emploi.Intitule;
            emploiExistant.DateDebutAffichage = emploi.DateDebutAffichage;
            emploiExistant.DateFinAffichage = emploi.DateFinAffichage;
            emploiExistant.Description = emploi.Description;

            await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Mise à jour de l'offre d'emploi avec l'Id {emploi.Id}.");

            return NoContent();
        }

        /// <summary>
        /// Supprime une offre d'emploi existante dans la base de données.
        /// </summary>
        /// <remarks>Je manque d'imagination</remarks>
        /// <param name="id">id de l'offre à supprimer</param>
        /// <response code="204">offre d'emploi supprimée avec succès</response>
        /// <response code="404">offre d'emploi non trouvée</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmploi(int id)
        {
            var emploi = await _context.Emplois.FindAsync(id);

            if (emploi == null)
            {
                return NotFound();
            }

            _context.Emplois.Remove(emploi);

            await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Suppression de l'offre d'emploi avec l'Id {emploi.Id}.");

            return NoContent();
        }


    }
}
