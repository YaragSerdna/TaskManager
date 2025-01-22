using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Helpers;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtHelper _jwtHelper;

        public AuthController(ApplicationDbContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            user.Password = HashPassword(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Usuario registrado exitosamente" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Los datos de inicio de sesión son inválidos." });
            }

            // Buscar al usuario por correo electrónico
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                return Unauthorized(new { message = "Usuario no encontrado" });

            // Verificar la contraseña
            if (!VerifyPassword(user.Password, request.Password))
                return Unauthorized(new { message = "Contraseña incorrecta" });

            // Generar un token JWT si la contraseña es válida
            return Ok(new { token = _jwtHelper.GenerateToken(user.Id.ToString(), user.Role) });
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                // Convertir la contraseña en un arreglo de bytes
                var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

                // Generar el hash
                var hashBytes = sha256.ComputeHash(passwordBytes);

                // Convertir el hash a una cadena en formato hexadecimal
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hashString;
            }
        }
        private bool VerifyPassword(string hash, string password)
        {
            // Hashear la contraseña ingresada por el usuario
            var hashedInputPassword = HashPassword(password);

            // Comparar el hash ingresado con el hash almacenado
            return SecureEquals(hashedInputPassword, hash);
        }

        //Protección contra Ataques de Timing
        private bool SecureEquals(string a, string b)
        {
            //Comparar el tamaño de las cadenas
            if (a.Length != b.Length)
                return false;

            //Comparar las cadenas por caracteres
            var result = true;
            for (int i = 0; i < a.Length; i++)
            {
                result &= a[i] == b[i];
            }
            return result;
        }
    }
}
