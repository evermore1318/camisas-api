using Azure.Core;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuario usuarioDB;

        public UsuariosController(IUsuario usuarioRepo)
        {
            usuarioDB = usuarioRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            return Ok(await Task.Run(() => usuarioDB.ListadoUsuarios()));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            return Ok(await Task.Run(() => usuarioDB.ObtenerPorID(id)));
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(UsuarioRequest request)
        {
            var usuario = new Usuario
            {
                Nombre = request.Nombre,
                Password = request.Password,
                IdRol = request.IdRol,
                Estado = request.Estado
            };

            return Ok(await Task.Run(() => usuarioDB.Registrar(usuario)));
        }

        [HttpPut]
        [Route("{id}/estado/{nuevoEstado}")]
        public async Task<IActionResult> ActualizarEstado(int id, string nuevoEstado)
        {
            return Ok(await Task.Run(() => usuarioDB.ActualizarEstado(id, nuevoEstado)));
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            return Ok(await Task.Run(() => usuarioDB.Eliminar(id)));
        }

        [HttpGet("roles")]
        public async Task<IActionResult> ListarRoles()
        {
            return Ok(await Task.Run(() => usuarioDB.ListadoRoles()));
        }
    }
}
