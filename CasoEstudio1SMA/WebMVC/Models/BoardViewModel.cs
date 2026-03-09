namespace WebMVC.Models
{
    public class BoardViewModel
    {
        public List<UserStoryViewModel> Backlog { get; set; } = new();
        public List<UserStoryViewModel> ToDo { get; set; } = new();
        public List<UserStoryViewModel> InProgress { get; set; } = new();
        public List<UserStoryViewModel> Done { get; set; } = new();
    }
}