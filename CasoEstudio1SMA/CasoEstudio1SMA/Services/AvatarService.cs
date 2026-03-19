using System.Net.Http.Json;

namespace CasoEstudio1SMA.Services
{
    public class AvatarService : IAvatarService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AvatarService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<int> GetRandomAvatarIdAsync()
        {
            var fallback = Random.Shared.Next(1, 152);

            try
            {
                var pokemonApiClient = _httpClientFactory.CreateClient("PokemonApi");
                var response = await pokemonApiClient.GetAsync("api/pokemon-id");
                if (!response.IsSuccessStatusCode)
                {
                    return fallback;
                }

                var remoteId = await response.Content.ReadFromJsonAsync<int>();
                if (remoteId is < 1 or > 151)
                {
                    return fallback;
                }

                return remoteId;
            }
            catch
            {
                return fallback;
            }
        }
    }
}