using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Models;

namespace TaskManagerFrontend.Pages.Tasks
{
    public class ViewModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public TaskItem Task { get; set; }

        public ViewModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task OnGetAsync(int id)
        {
            var client = _clientFactory.CreateClient("TaskManagerAPI");
            Task = await client.GetFromJsonAsync<TaskItem>($"tasks/{id}");
        }
    }
}
