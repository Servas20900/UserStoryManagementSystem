using CasoEstudio1SMA.DTOs;
using CasoEstudio1SMA.Services;
using Microsoft.AspNetCore.Mvc;

namespace CasoEstudio1SMA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserResponseDto>>> GetAll()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserResponseDto>> GetById(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user is null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> Create([FromBody] UserCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            if (created is null)
            {
                return BadRequest(new { message = "Ya existe un usuario con ese email" });
            }

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
    }
}