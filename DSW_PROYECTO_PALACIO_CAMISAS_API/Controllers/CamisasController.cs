using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models.DTOs;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CamisasController : ControllerBase
    {
       
            private readonly ICamisa camisaDB;
            private readonly IMarca marcaDB;
            private readonly IEstante estanteDB;

            public CamisasController(ICamisa camisaRepo, IMarca marcaRepo, IEstante estanteRepo)
            {
                camisaDB = camisaRepo;
                marcaDB = marcaRepo;
                estanteDB = estanteRepo;
            }
        [HttpGet]
        public async Task<IActionResult> Listar(
          [FromQuery] int marcaId = 0,
          [FromQuery] string tipo = "",
          [FromQuery] string talla = "",
          [FromQuery] string manga = "",
          [FromQuery] string color = "")
        {
            try
            {
                // Obtener todas las camisas (filtrado se hace aquí por simplicidad)
                var camisas = await Task.Run(() => camisaDB.Listado());

                // Aplicar filtros si están presentes
                var camisasFiltradas = camisas.AsQueryable();

                if (marcaId > 0)
                    camisasFiltradas = camisasFiltradas.Where(c => c.id_marca == marcaId);

                if (!string.IsNullOrEmpty(tipo))
                    camisasFiltradas = camisasFiltradas.Where(c => c.descripcion.Contains(tipo, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrEmpty(talla))
                    camisasFiltradas = camisasFiltradas.Where(c => c.talla.Equals(talla, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrEmpty(manga))
                    camisasFiltradas = camisasFiltradas.Where(c => c.manga.Contains(manga, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrEmpty(color))
                    camisasFiltradas = camisasFiltradas.Where(c => c.color.Contains(color, StringComparison.OrdinalIgnoreCase));

                // DEVOLVER DIRECTAMENTE LAS CAMISAS, NO DTO
                return Ok(camisasFiltradas.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var camisa = await Task.Run(() => camisaDB.ObtenerPorID(id));
                if (camisa == null)
                    return NotFound();

                // DEVOLVER DIRECTAMENTE LA CAMISA, NO DTO
                return Ok(camisa);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] Camisa camisa)
        {
            try
            {
                var nuevaCamisa = await Task.Run(() => camisaDB.Registrar(camisa));
                return Ok(nuevaCamisa);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Camisa camisa)
        {
            try
            {
                camisa.id_camisa = id;
                var camisaActualizada = await Task.Run(() => camisaDB.Actualizar(camisa));
                return Ok(camisaActualizada);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var eliminado = await Task.Run(() => camisaDB.Eliminar(id));
                return Ok(eliminado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}