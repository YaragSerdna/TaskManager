﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null || !VerifyPassword(user.Password, password))
                return Unauthorized();

            var token = _jwtHelper.GenerateToken(user.Id.ToString(), user.Role);
            return Ok(new { token });
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
        private bool VerifyPassword(string hashedPassword, string inputPassword)
        {
            var inputHash = HashPassword(inputPassword);
            return inputHash == hashedPassword;
        }
    }
}
