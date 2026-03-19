using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserStoryOrchestrationService _service;

        public UsersController(IUserStoryOrchestrationService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateUserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _service.CreateUserAsync(model);
                TempData["UserSuccess"] = "Usuario creado correctamente";
                return RedirectToAction(nameof(Create));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "No se pudo crear el usuario");
                return View(model);
            }
        }
    }
}