using System.Net.Http.Json;
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

            return await response.Content.ReadFromJsonAsync<List<UserStoryViewModel>>() ?? new List<UserStoryViewModel>();
        }

        public async Task CreateAsync(CreateUserStoryViewModel model)
        {
            var estimationApiClient = _httpClientFactory.CreateClient("EstimationApi");
            var estimationResponse = await estimationApiClient.GetAsync("api/estimation");
            estimationResponse.EnsureSuccessStatusCode();

            var estimation = await estimationResponse.Content.ReadFromJsonAsync<int>();

            var payload = new
            {
                model.Titulo,
                model.Descripcion,
                model.AsignadoA,
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
                story.AsignadoA,
                Estado = normalizedStatus,
                story.Estimacion
            };

            var updateResponse = await storyApiClient.PutAsJsonAsync($"api/userstories/{id}", payload);
            updateResponse.EnsureSuccessStatusCode();
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
    }
}