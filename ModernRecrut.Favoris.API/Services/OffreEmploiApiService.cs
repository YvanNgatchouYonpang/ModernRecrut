using ModernRecrut.Favoris.API.DTOs;

namespace ModernRecrut.Favoris.API.Services
{
    public class OffreEmploiApiService
    {
        private readonly HttpClient _httpClient;

        public OffreEmploiApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Récupérer toutes les offres
        public async Task<List<EmploiDto>> GetToutesLesOffres()
        {
            var offres = await _httpClient
                .GetFromJsonAsync<List<EmploiDto>>("api/Emplois");

            return offres ?? new List<EmploiDto>();
        }

        // Récupérer une offre à partir de son identifiant
        public async Task<EmploiDto?> GetEmploiParId(int id)
        {
            var offres = await GetToutesLesOffres();

            return offres.FirstOrDefault(e => e.Id == id);
        }
    }
}
