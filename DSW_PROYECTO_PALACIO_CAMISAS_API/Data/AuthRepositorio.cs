using Microsoft.AspNetCore.Mvc;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Data;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models.DTOs;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authRepositorio;

        public AuthController(IAuth authRepositorio)
        {
            _authRepositorio = authRepositorio;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var usuario = _authRepositorio.Login(request.Usuario, request.Clave);

            if (usuario == null)
                return Unauthorized("Credenciales incorrectas");

            return Ok(usuario);
        }
    }

    public class LoginRequest
    {
        public string Usuario { get; set; }
        public string Clave { get; set; }
    }
}