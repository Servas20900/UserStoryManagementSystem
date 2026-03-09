using CasoEstudio1SMA.DTOs;
using CasoEstudio1SMA.Services;
using Microsoft.AspNetCore.Mvc;

namespace CasoEstudio1SMA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserStoriesController : ControllerBase
    {
        private readonly IUserStoryService _service;

        public UserStoriesController(IUserStoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserStoryResponseDto>>> GetAll()
        {
            var stories = await _service.GetAllAsync();
            return Ok(stories);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserStoryResponseDto>> GetById(int id)
        {
            var story = await _service.GetByIdAsync(id);
            if (story is null)
            {
                return NotFound(new { message = "User Story no encontrada" });
            }

            return Ok(story);
        }

        [HttpPost]
        public async Task<ActionResult<UserStoryResponseDto>> Create([FromBody] UserStoryCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserStoryUpdateDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (!updated)
            {
                return NotFound(new { message = "User Story no encontrada" });
            }

            return Ok(new { message = "User Story actualizada correctamente" });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = "User Story no encontrada" });
            }

            return Ok(new { message = "User Story eliminada correctamente" });
        }
    }
}