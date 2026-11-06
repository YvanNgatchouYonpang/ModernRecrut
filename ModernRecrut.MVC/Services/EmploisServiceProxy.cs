using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;

namespace ModernRecrut.MVC.Services
{
    public class EmploisServiceProxy : IEmploisService
    {
        private readonly HttpClient _httpClient;
        private readonly string _emploisApiUrl="api/emplois/";

        public EmploisServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Ajouter(Emplois emplois)
        {
            var response = await _httpClient.PostAsJsonAsync(_emploisApiUrl, emplois);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erreur lors de l'appel à l'API : {response.StatusCode}");
            }
        }

        public async Task<Emplois> ObtenirSelonId(int id)
        {
            var response = await _httpClient.GetAsync($"{_emploisApiUrl}{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erreur lors de l'appel à l'API : {response.StatusCode}");
            }
            var emplois = await response.Content.ReadFromJsonAsync<Emplois>();
            return emplois ?? new Emplois();
        }

        public async Task Modifier(Emplois emplois)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_emploisApiUrl}{emplois.Id}", emplois);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erreur lors de l'appel à l'API : {response.StatusCode}");
            }
        }

        public async Task Supprimer(Emplois emplois)
        {
            var response = await _httpClient.DeleteAsync($"{_emploisApiUrl}/{emplois.Id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erreur lors de l'appel à l'API : {response.StatusCode}");
            }

        }   

        public async Task<IEnumerable<Emplois>> ObtenirTout()
        {
           var response = await _httpClient.GetAsync(_emploisApiUrl);

            if(!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erreur lors de l'appel à l'API : {response.StatusCode}");
            }

            var emplois = await response.Content.ReadFromJsonAsync<IEnumerable<Emplois>>();
            return emplois ?? Enumerable.Empty<Emplois>();
        }
    }
}
