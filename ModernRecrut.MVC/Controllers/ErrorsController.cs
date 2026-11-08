using Microsoft.AspNetCore.Mvc;
using ModernRecrut.MVC.Models;
using System.Diagnostics;

namespace ModernRecrut.MVC.Controllers
{
    public class ErrorsController : Controller
    {
        [Route("Errors")]
        public IActionResult Index()
        {
            Response.StatusCode = 500;

            var model = new HttpCodeErrorViewModel
            {
                RequestId = HttpContext.TraceIdentifier,
                StatusCode = 500,
                ErrorMessage = "Une erreur inattendue est survenue."
            };

            return View("HttpError", model);
        }

        public IActionResult HttpError(int statusCode)
        {
            string? errorMessage = "Une erreur s'est produit, veuillez reessayer plus tard.";

            HttpCodeErrorViewModel errorViewModel = new HttpCodeErrorViewModel
            {
                ErrorMessage = errorMessage,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = statusCode

            };

            if (statusCode == 404)
                return View("NotFound", errorViewModel);

            return View("HttpError", errorViewModel);
        }
    }
}
