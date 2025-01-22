using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Models;

namespace TaskManagerFrontend.Pages.Users
{
    public class ViewModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public User User { get; set; }

        public ViewModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task OnGetAsync(int id)
        {
            var client = _clientFactory.CreateClient("TaskManagerAPI");
            User = await client.GetFromJsonAsync<User>($"users/{id}");
        }
    }
}
