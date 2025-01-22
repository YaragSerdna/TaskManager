using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaskManagerFrontend.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; } // Para mensajes adicionales opcionales

        public LoginModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Retorna la página con los errores de validación
            }

            try
            {
                var client = _clientFactory.CreateClient("TaskManagerAPI");
                var response = await client.PostAsJsonAsync("auth/login", new { Email, Password });

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();

                    // Configurar la cookie
                    HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions
                    {
                        HttpOnly = true, // Aumenta la seguridad evitando acceso desde JavaScript
                        Secure = true,   // Requiere HTTPS
                        SameSite = SameSiteMode.Strict, // Evita el uso en contextos de terceros
                        Expires = DateTime.UtcNow.AddHours(2) // Expira en 2 horas
                    });

                    return RedirectToPage("/Tasks/Index");
                }

                // Si el backend devuelve un error, muestra un mensaje en la página
                ModelState.AddModelError(string.Empty, "Credenciales inválidas. Verifique su correo y contraseña.");
            }
            catch (Exception ex)
            {
                // Capturar errores de red u otros problemas
                ModelState.AddModelError(string.Empty, "Ocurrió un error al intentar iniciar sesión. Intente más tarde.");
                ErrorMessage = ex.Message; // Para depuración opcional
            }

            return Page(); // Retorna a la misma página con el error
        }
    }
}
