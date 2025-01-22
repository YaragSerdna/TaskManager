using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaskManagerFrontend.Pages.Tasks
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        public string Status { get; set; }

        public CreateModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = _clientFactory.CreateClient("TaskManagerAPI");
            var response = await client.PostAsJsonAsync("tasks", new { Title, Description, Status });

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Tasks/Index");
            }

            ModelState.AddModelError(string.Empty, "Error al crear la tarea. Intente nuevamente.");
            return Page();
        }
    }
}
