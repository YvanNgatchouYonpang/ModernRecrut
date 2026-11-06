using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace ModernRecrut.MVC.Services
{
    public class FavorisServiceProxy : IFavorisService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _favorisApiUrl="api/favoris/";

        public FavorisServiceProxy(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private void AjouterAdresseIpDansHeaders()
        {
            var ip = _httpContextAccessor.HttpContext?
                .Connection.RemoteIpAddress?
                .MapToIPv4()
                .ToString();
            if (!string.IsNullOrEmpty(ip))
            {
                _httpClient.DefaultRequestHeaders.Remove("X-Forwarded-For");
                _httpClient.DefaultRequestHeaders.Add("X-Forwarded-For", ip);
            }
        }


        public async Task<bool> Ajouter(int offreId)
        {
            AjouterAdresseIpDansHeaders();

            var response = await _httpClient.PostAsJsonAsync(_favorisApiUrl, new { OffreId = offreId });
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erreur lors de l'appel à l'API : {response.StatusCode}");
            }

            return response.IsSuccessStatusCode;

        }

        public async Task<bool> Supprimer(int emploiId)
        {
            AjouterAdresseIpDansHeaders();

            var response = await _httpClient.DeleteAsync($"{_favorisApiUrl}{emploiId}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erreur lors de l'appel à l'API : {response.StatusCode}");
            }
            return response.IsSuccessStatusCode;
        }

        public async Task<Favoris> ObtenirTout()
        {
            AjouterAdresseIpDansHeaders();

            var response = await _httpClient.GetAsync(_favorisApiUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erreur lors de l'appel à l'API : {response.StatusCode}");
            }
            var favoris = await response.Content.ReadFromJsonAsync<Favoris>();

            return favoris ?? new Favoris();
        }
    }
}
