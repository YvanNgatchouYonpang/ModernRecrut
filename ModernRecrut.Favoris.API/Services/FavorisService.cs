using Microsoft.Extensions.Caching.Memory;
using ModernRecrut.Favoris.API.DTOs;
using ModernRecrut.Favoris.API.Models;
using System.Text.Json;

namespace ModernRecrut.Favoris.API.Services
{
    public class FavorisService
    {
        private readonly IMemoryCache _cache;
        private readonly OffreEmploiApiService _offreApiService;

        public FavorisService(
            IMemoryCache cache,
            OffreEmploiApiService offreApiService)
        {
            _cache = cache;
            _offreApiService = offreApiService;
        }

        // Générer une clé à partir de l'adresse IP
        private string GenererCle(string adresseIP)
        {
            return adresseIP;
        }

        // Récupérer les favoris d'un utilisateur
        public Favori GetFavoris(string adresseIP)
        {
            string cle = GenererCle(adresseIP);

            if (_cache.TryGetValue(cle, out Favori? favori)
                && favori != null)
            {
                return favori;
            }

            return new Favori
            {
                Cle = adresseIP,
                ListeEmplois = new List<EmploiDto>()
            };
        }

        // Enregistrer les favoris dans le cache
        private void Enregistrer(Favori favori)
        {
            string cle = GenererCle(favori.Cle);

            // Convertir les favoris en JSON
            string donnees = JsonSerializer.Serialize(favori);

            // Chaque caractère représente une unité
            long taille = donnees.Length;   

            var options = new MemoryCacheEntryOptions()
                .SetSize(taille)
                .SetSlidingExpiration(TimeSpan.FromHours(6))
                .SetAbsoluteExpiration(TimeSpan.FromHours(24));

            _cache.Set(cle, favori, options);
        }

        // GET : récupérer les favoris
        public Favori ObtenirFavoris(string adresseIP)
        {
            // TryGetValue renouvelle automatiquement
            // le délai d'expiration glissant.
            return GetFavoris(adresseIP);
        }

        // POST : ajouter une offre aux favoris
        public async Task<bool> AjouterFavori(
            string adresseIP,
            int emploiId)
        {
            Favori favori = GetFavoris(adresseIP);

            // Vérifier si l'offre existe déjà dans les favoris
            if (favori.ListeEmplois.FirstOrDefault(e => e.Id == emploiId) != null)
            {
                return false;
            }

            // Récupérer l'offre depuis OffreEmploiAPI
            EmploiDto? emploi =
                await _offreApiService.GetEmploiParId(emploiId);

            if (emploi == null)
            {
                return false;
            }

            favori.ListeEmplois.Add(emploi);

            Enregistrer(favori);

            return true;
        }

        // PUT : remplacer une offre favorite par une autre
        public async Task<bool> ModifierFavori(
            string adresseIP,
            int ancienId,
            int nouvelId)
        {
            Favori favori = GetFavoris(adresseIP);

            EmploiDto? ancienEmploi =
                favori.ListeEmplois.FirstOrDefault(e => e.Id == ancienId);

            if (ancienEmploi == null)
            {
                return false;
            }

            // Vérifier que la nouvelle offre n'est pas déjà favorite
            if (favori.ListeEmplois.Any(e => e.Id == nouvelId))
            {
                return false;
            }

            EmploiDto? nouvelEmploi =
                await _offreApiService.GetEmploiParId(nouvelId);

            if (nouvelEmploi == null)
            {
                return false;
            }

            int index = favori.ListeEmplois.IndexOf(ancienEmploi);

            favori.ListeEmplois[index] = nouvelEmploi;

            Enregistrer(favori);

            return true;
        }

        // DELETE : supprimer une offre des favoris
        public bool SupprimerFavori(
            string adresseIP,
            int emploiId)
        {
            Favori favori = GetFavoris(adresseIP);

            EmploiDto? emploi =
                favori.ListeEmplois.FirstOrDefault(e => e.Id == emploiId);

            if (emploi == null)
            {
                return false;
            }

            favori.ListeEmplois.Remove(emploi);

            Enregistrer(favori);

            return true;
        }
    }
}

