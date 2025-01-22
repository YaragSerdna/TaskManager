using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Models;

namespace TaskManagerFrontend.Pages.Tasks
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        [BindProperty]
        public TaskItem Task { get; set; }

        public DeleteModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        // Muestra la tarea que se eliminará
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _clientFactory.CreateClient("TaskManagerAPI");
            Task = await client.GetFromJsonAsync<TaskItem>($"tasks/{id}");

            if (Task == null)
            {
                return RedirectToPage("/Tasks/Index");
            }

            return Page();
        }

        // Maneja la eliminación
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _clientFactory.CreateClient("TaskManagerAPI");
            var response = await client.DeleteAsync($"tasks/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Tasks/Index");
            }

            ModelState.AddModelError(string.Empty, "Error al eliminar la tarea. Intente nuevamente.");
            return Page();
        }
    }
}
