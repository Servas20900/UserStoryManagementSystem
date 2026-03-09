using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebMVC.Models;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserStoryOrchestrationService _userStoryService;

        public HomeController(ILogger<HomeController> logger, IUserStoryOrchestrationService userStoryService)
        {
            _logger = logger;
            _userStoryService = userStoryService;
        }

        public async Task<IActionResult> Index()
        {
            var stories = await _userStoryService.GetAllAsync();
            var board = BuildBoard(stories);
            return View("~/Views/Board/Index.cshtml", board);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserStoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var stories = await _userStoryService.GetAllAsync();
                var board = BuildBoard(stories);
                return View("~/Views/Board/Index.cshtml", board);
            }

            await _userStoryService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string newStatus)
        {
            await _userStoryService.UpdateStatusAsync(id, newStatus);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private static BoardViewModel BuildBoard(List<UserStoryViewModel> stories)
        {
            var board = new BoardViewModel();

            foreach (var story in stories)
            {
                switch (story.Estado.Trim())
                {
                    case "Backlog":
                        board.Backlog.Add(story);
                        break;
                    case "To Do":
                    case "ToDo":
                        board.ToDo.Add(story);
                        break;
                    case "In Progress":
                    case "InProgress":
                        board.InProgress.Add(story);
                        break;
                    case "Done":
                        board.Done.Add(story);
                        break;
                    default:
                        board.Backlog.Add(story);
                        break;
                }
            }

            return board;
        }
    }
}
