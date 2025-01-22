using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Models;

namespace TaskManagerFrontend.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public List<User> Users { get; set; } = new List<User>();

        public IndexModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task OnGetAsync()
        {
            var client = _clientFactory.CreateClient("TaskManagerAPI");
            Users = await client.GetFromJsonAsync<List<User>>("users");
        }
    }
}
