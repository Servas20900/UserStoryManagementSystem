using System.Net.Http.Json;
using System.Text.Json;
using WebMVC.Models;

namespace WebMVC.Services
{
    public class UserStoryOrchestrationService : IUserStoryOrchestrationService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserStoryOrchestrationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<UserStoryViewModel>> GetAllAsync()
        {
            var storyApiClient = _httpClientFactory.CreateClient("StoryApi");
            var response = await storyApiClient.GetAsync("api/userstories");
            response.EnsureSuccessStatusCode();

            var stories = await response.Content.ReadFromJsonAsync<List<UserStoryViewModel>>() ?? new List<UserStoryViewModel>();

            foreach (var story in stories)
            {
                story.AvatarImageUrl = await ResolveAvatarImageUrlAsync(story.AvatarId);
            }

            return stories;
        }

        public async Task<UserStoryViewModel?> GetByIdAsync(int id)
        {
            var storyApiClient = _httpClientFactory.CreateClient("StoryApi");
            var response = await storyApiClient.GetAsync($"api/userstories/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var story = await response.Content.ReadFromJsonAsync<UserStoryViewModel>();
            if (story is null)
            {
                return null;
            }

            story.AvatarImageUrl = await ResolveAvatarImageUrlAsync(story.AvatarId);
            return story;
        }

        public async Task CreateAsync(CreateUserStoryViewModel model)
        {
            var estimationApiClient = _httpClientFactory.CreateClient("EstimationApi");
            var estimation = await GetEstimationOrFallbackAsync(estimationApiClient);

            var payload = new
            {
                model.Titulo,
                model.Descripcion,
                model.UserId,
                Estado = string.IsNullOrWhiteSpace(model.Estado) ? "Backlog" : model.Estado,
                Estimacion = estimation
            };

            var storyApiClient = _httpClientFactory.CreateClient("StoryApi");
            var createResponse = await storyApiClient.PostAsJsonAsync("api/userstories", payload);
            createResponse.EnsureSuccessStatusCode();
        }

        public async Task UpdateStatusAsync(int id, string newStatus)
        {
            var storyApiClient = _httpClientFactory.CreateClient("StoryApi");

            var getResponse = await storyApiClient.GetAsync($"api/userstories/{id}");
            getResponse.EnsureSuccessStatusCode();

            var story = await getResponse.Content.ReadFromJsonAsync<UserStoryViewModel>();
            if (story is null)
            {
                return;
            }

            var normalizedStatus = NormalizeStatus(newStatus);

            var payload = new
            {
                story.Titulo,
                story.Descripcion,
                story.UserId,
                Estado = normalizedStatus,
                story.Estimacion
            };

            var updateResponse = await storyApiClient.PutAsJsonAsync($"api/userstories/{id}", payload);
            updateResponse.EnsureSuccessStatusCode();
        }

        public async Task<bool> UpdateAsync(EditUserStoryViewModel model)
        {
            var storyApiClient = _httpClientFactory.CreateClient("StoryApi");
            var payload = new
            {
                model.Titulo,
                model.Descripcion,
                model.UserId,
                model.Estado,
                model.Estimacion
            };

            var response = await storyApiClient.PutAsJsonAsync($"api/userstories/{model.Id}", payload);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }

            response.EnsureSuccessStatusCode();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var storyApiClient = _httpClientFactory.CreateClient("StoryApi");
            var response = await storyApiClient.DeleteAsync($"api/userstories/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }

            response.EnsureSuccessStatusCode();
            return true;
        }

        public async Task<List<UserViewModel>> GetUsersAsync()
        {
            var storyApiClient = _httpClientFactory.CreateClient("StoryApi");
            var response = await storyApiClient.GetAsync("api/users");

            if (!response.IsSuccessStatusCode)
            {
                return new List<UserViewModel>();
            }

            return await response.Content.ReadFromJsonAsync<List<UserViewModel>>() ?? new List<UserViewModel>();
        }

        public async Task CreateUserAsync(CreateUserViewModel model)
        {
            var storyApiClient = _httpClientFactory.CreateClient("StoryApi");
            var payload = new
            {
                model.Nombre,
                model.Apellidos,
                model.Email
            };

            var response = await storyApiClient.PostAsJsonAsync("api/users", payload);
            response.EnsureSuccessStatusCode();
        }

        private static string NormalizeStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return "Backlog";
            }

            return status.Trim() switch
            {
                "ToDo" => "To Do",
                "InProgress" => "In Progress",
                "Backlog" => "Backlog",
                "To Do" => "To Do",
                "In Progress" => "In Progress",
                "Done" => "Done",
                _ => "Backlog"
            };
        }

        private static int GetLocalEstimationFallback()
        {
            int[] fibonacciValues = { 2, 3, 5, 8, 13 };
            return fibonacciValues[Random.Shared.Next(fibonacciValues.Length)];
        }

        private static string GetImageFallback(int avatarId)
        {
            var normalized = avatarId is < 1 or > 151 ? 1 : avatarId;
            return $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{normalized}.png";
        }

        private async Task<int> GetEstimationOrFallbackAsync(HttpClient estimationApiClient)
        {
            try
            {
                var estimationResponse = await estimationApiClient.GetAsync("api/estimation");
                if (!estimationResponse.IsSuccessStatusCode)
                {
                    return GetLocalEstimationFallback();
                }

                var estimation = await estimationResponse.Content.ReadFromJsonAsync<int>();
                return estimation is 2 or 3 or 5 or 8 or 13 ? estimation : GetLocalEstimationFallback();
            }
            catch
            {
                return GetLocalEstimationFallback();
            }
        }

        private async Task<string> ResolveAvatarImageUrlAsync(int avatarId)
        {
            try
            {
                var normalized = avatarId is < 1 or > 151 ? 1 : avatarId;
                var pokeApiClient = _httpClientFactory.CreateClient("PokeApi");
                var response = await pokeApiClient.GetAsync($"api/v2/pokemon/{normalized}");
                if (!response.IsSuccessStatusCode)
                {
                    return GetImageFallback(normalized);
                }

                var payload = await response.Content.ReadAsStringAsync();
                using var document = JsonDocument.Parse(payload);

                if (document.RootElement.TryGetProperty("sprites", out var sprites)
                    && sprites.TryGetProperty("front_default", out var frontDefault)
                    && frontDefault.ValueKind == JsonValueKind.String)
                {
                    var url = frontDefault.GetString();
                    if (!string.IsNullOrWhiteSpace(url))
                    {
                        return url;
                    }
                }

                return GetImageFallback(normalized);
            }
            catch
            {
                return GetImageFallback(avatarId);
            }
        }
    }
}