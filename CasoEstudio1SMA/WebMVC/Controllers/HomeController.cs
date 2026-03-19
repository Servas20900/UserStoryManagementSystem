using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var users = await _userStoryService.GetUsersAsync();
            var board = BuildBoard(stories, users);
            return View("~/Views/Board/Index.cshtml", board);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(Prefix = "CreateStory")] CreateUserStoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var stories = await _userStoryService.GetAllAsync();
                var users = await _userStoryService.GetUsersAsync();
                var board = BuildBoard(stories, users);
                board.CreateStory.Titulo = model.Titulo;
                board.CreateStory.Descripcion = model.Descripcion;
                board.CreateStory.UserId = model.UserId;
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var story = await _userStoryService.GetByIdAsync(id);
            if (story is null)
            {
                return RedirectToAction(nameof(Index));
            }

            var users = await _userStoryService.GetUsersAsync();
            var model = new EditUserStoryViewModel
            {
                Id = story.Id,
                Titulo = story.Titulo,
                Descripcion = story.Descripcion,
                UserId = story.UserId,
                Estado = story.Estado,
                Estimacion = story.Estimacion,
                Users = users
                    .Select(user => new SelectListItem
                    {
                        Value = user.Id.ToString(),
                        Text = $"{user.Nombre} {user.Apellidos}".Trim()
                    })
                    .ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserStoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var users = await _userStoryService.GetUsersAsync();
                model.Users = users
                    .Select(user => new SelectListItem
                    {
                        Value = user.Id.ToString(),
                        Text = $"{user.Nombre} {user.Apellidos}".Trim()
                    })
                    .ToList();

                return View(model);
            }

            await _userStoryService.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _userStoryService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private static BoardViewModel BuildBoard(List<UserStoryViewModel> stories, List<UserViewModel> users)
        {
            var board = new BoardViewModel
            {
                CreateStory = new CreateUserStoryViewModel
                {
                    Estado = "Backlog",
                    Users = users
                        .Select(user => new SelectListItem
                        {
                            Value = user.Id.ToString(),
                            Text = $"{user.Nombre} {user.Apellidos}".Trim()
                        })
                        .ToList()
                }
            };

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
