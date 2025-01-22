using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    [Authorize] // Requiere autenticación para acceder
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener todas las tareas
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await _context.Tasks.ToListAsync();
            return Ok(tasks);
        }

        // Obtener una tarea específica por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound(new { message = "Tarea no encontrada" });

            return Ok(task);
        }

        // Crear una nueva tarea
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem task)
        {
            if (task == null)
                return BadRequest(new { message = "Datos inválidos" });

            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        // Actualizar una tarea existente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItem task)
        {
            if (id != task.Id)
                return BadRequest(new { message = "El ID de la tarea no coincide" });

            var existingTask = await _context.Tasks.FindAsync(id);
            if (existingTask == null)
                return NotFound(new { message = "Tarea no encontrada" });

            // Actualizar los campos de la tarea
            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Status = task.Status;
            existingTask.AssignedUserId = task.AssignedUserId;
            existingTask.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent(); // Respuesta estándar para actualizaciones exitosas
        }

        // Eliminar una tarea
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound(new { message = "Tarea no encontrada" });

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent(); // Respuesta estándar para eliminaciones exitosas
        }
    }
}

