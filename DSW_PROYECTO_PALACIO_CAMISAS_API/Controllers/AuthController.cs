using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth authRepo;

        public AuthController(IAuth repo)
        {
            authRepo = repo;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await Task.Run(() => authRepo.Login(request.Usuario, request.Clave));

            if (usuario == null)
                return Unauthorized(new { mensaje = "Usuario o clave incorrectos" });

            return Ok(usuario);
        }
    }

}
