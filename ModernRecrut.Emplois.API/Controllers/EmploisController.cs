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
        public EmploisController(EmploiDbContext context)
        {
            _context = context;
        }

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

            return CreatedAtAction(
                nameof(GetEmploi),
                new { id = emploi.Id },
                emploi);
        }

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

            return NoContent();
        }

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

            return NoContent();
        }


    }
}
