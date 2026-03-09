namespace WebMVC.Models
{
    public class ColumnViewModel
    {
        public string Name { get; set; } = string.Empty;
        public IEnumerable<UserStoryViewModel> Stories { get; set; } = Enumerable.Empty<UserStoryViewModel>();
    }
}
