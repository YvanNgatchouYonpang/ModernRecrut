using Microsoft.AspNetCore.Mvc;
using ModernRecrut.Documents.API.Interfaces;
using ModernRecrut.Documents.API.Models;
using System.Globalization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ModernRecrut.Documents.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly ILogger<DocumentsController> _logger;

        public DocumentsController( IDocumentService documentService, ILogger<DocumentsController> logger)
        {
            _documentService = documentService;
            _logger = logger;
        }

        /// <summary>
        /// Retourne les documents de l'utilisateur.
        /// </summary>
        /// <remarks>
        /// Je manque d'imagination
        /// 
        /// Exemple de requete
        /// 
        /// GET / Documents
        /// {
        ///      "idUtilisateur": 1
        /// }
        /// </remarks>
        /// <response code="200">documents trouvés et retournés</response>
        /// <response code="404">documents introuvables</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // GET api/<DocumentsController>/5
        [HttpGet("{idUtilisateur}")]
        public IActionResult ObternirDocuments(int idUtilisateur)
        {
            List<string> documents = _documentService.ObtenirDocmuments(idUtilisateur);
            _logger.LogInformation("Consultation de la liste des document de l'utilisateur avec l'ID : {idUtilisateur}",idUtilisateur);
            return Ok(documents);
        }


        /// <summary>
        /// Enregistre un document.
        /// </summary>
        /// <remarks>
        /// Je manque d'imagination
        /// 
        /// Exemple de requete
        /// 
        /// POST / Documents
        /// {
        ///      "idUtilisateur": 1,
        ///      "typeDocument": "CV,
        ///      "Fichier": "choisir le fichier"
        /// }
        /// </remarks>
        /// <response code="201">document enregistré avec succès</response>
        /// <response code="401">erreur de validation</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>

        //Eregister un document
        // POST api/<DocumentsController>
        [HttpPost]
        public async Task<IActionResult> Enregister([FromForm] Document document)
        {
            if(document.Fichier==null || document.Fichier.Length == 0)
            {
                return BadRequest("Le fichier est vide ou invalide.");
            }

            //verification de l'extension
            string extension = Path.GetExtension(document.Fichier.FileName).ToLower();
            if(extension!=".pdf" && extension != ".doc" && extension != ".docx")
            {
                return BadRequest("Seuls les documents PDF et Word sont acceptes");
            }

            string nomDocument = await _documentService.EnregistrerDocument(document);

            _logger.LogInformation("Enregistrement d'un document de type {typeDocument}.",document.TypeDocument);

            // Retourner l'URL d'acces au document
            var fileUrl = $"{Request.Scheme}://{Request.Host}/documents/{nomDocument}";

            return Ok(new { message = "Document enregistre avec succes", url = fileUrl });
        }




       
    }
}
