using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;

namespace TaskManager.Controllers
{
    [Route("api/users")] 
    [ApiController] 
    //[Authorize(Roles = "Admin")] // Solo accesible para administradores
    public class UsersController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener todos los usuarios
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                .Select(user => new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Role
                })
                .ToListAsync();

            return Ok(users);
        }

        // Obtener un usuario específico por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users
                .Select(user => new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Role
                })
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound(new { message = "Usuario no encontrado" });

            return Ok(user);
        }
    }
}
