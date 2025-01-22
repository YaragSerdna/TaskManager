using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Models;

namespace TaskManagerFrontend.Pages.Tasks
{
    public class UpdateModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        public string Status { get; set; }

        public string ErrorMessage { get; set; } // Para mensajes adicionales opcionales

        public UpdateModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task OnGetAsync(int id)
        {
            var client = _clientFactory.CreateClient("TaskManagerAPI");
            var task = await client.GetFromJsonAsync<TaskItem>($"tasks/{id}");

            if (task != null)
            {
                Id = task.Id;
                Title = task.Title;
                Description = task.Description;
                Status = task.Status;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = _clientFactory.CreateClient("TaskManagerAPI");
            var response = await client.PutAsJsonAsync($"tasks/{Id}", new { Id, Title, Description, Status });

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Tasks/Index");
            }

            ModelState.AddModelError(string.Empty, "Error al actualizar la tarea. Intente nuevamente.");
            return Page();
        }
    }
}
