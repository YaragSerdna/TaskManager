using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerFrontend.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public RegisterModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [BindProperty]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Name { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Debe ser un correo electrónico válido")]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Devuelve la página con los mensajes de error si los datos son inválidos
                return Page();
            }

            var client = _clientFactory.CreateClient("TaskManagerAPI");
            var response = await client.PostAsJsonAsync("auth/register", new { Name, Email, Password, Role = "User" });

            if (response.IsSuccessStatusCode)
                return RedirectToPage("/Auth/Login");

            ModelState.AddModelError(string.Empty, "Error al registrarse");
            return Page();
        }
    }
}
