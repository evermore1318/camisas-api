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
           [FromQuery] int? marcaId,
           [FromQuery] string? tipo,
           [FromQuery] string? talla,
           [FromQuery] string? manga,
           [FromQuery] string? color)
        {
            var camisas = await Task.Run(() => camisaDB.Listado()); 
            // Filtros en memoria e ignoramos mayuscs
            if (marcaId.HasValue)
                camisas = camisas.Where(c => c.id_marca == marcaId.Value).ToList();

            if (!string.IsNullOrWhiteSpace(tipo))
                camisas = camisas.Where(c =>
                    (c.descripcion ?? string.Empty)
                    .Contains(tipo, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(talla))
                camisas = camisas.Where(c =>
                    string.Equals(c.talla ?? string.Empty, talla, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(manga))
                camisas = camisas.Where(c =>
                    string.Equals(c.manga ?? string.Empty, manga, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(color))
                camisas = camisas.Where(c =>
                    (c.color ?? string.Empty)
                    .Contains(color, StringComparison.OrdinalIgnoreCase)).ToList();

            var camisasDto = camisas.Select(c => new CamisaDto
            {
                id_camisa = c.id_camisa,
                descripcion = c.descripcion,
                marca = c.marca_nombre ?? "",
                color = c.color,
                talla = c.talla,
                manga = c.manga,
                stock = c.stock,
                precio_costo = c.precio_costo,
                precio_venta = c.precio_venta,
                estante = c.estante_descripcion ?? "",
                estado = c.estado,
                id_marca = c.id_marca,
                id_estante = c.id_estante
            }).ToList();

            return Ok(camisasDto);
        }

        [HttpGet("{id}")]
            public async Task<IActionResult> ObtenerPorId(int id)
            {
                var camisa = await Task.Run(() => camisaDB.ObtenerPorID(id));
                if (camisa == null)
                    return NotFound();

                var camisaDto = new CamisaDto
                {
                    id_camisa = camisa.id_camisa,
                    descripcion = camisa.descripcion,
                    marca = camisa.marca_nombre ?? "",
                    color = camisa.color,
                    talla = camisa.talla,
                    manga = camisa.manga,
                    stock = camisa.stock,
                    precio_costo = camisa.precio_costo,
                    precio_venta = camisa.precio_venta,
                    estante = camisa.estante_descripcion ?? "",
                    estado = camisa.estado,
                    id_marca = camisa.id_marca,
                    id_estante = camisa.id_estante
                };

                return Ok(camisaDto);
            }

            [HttpPost]
            public async Task<IActionResult> Registrar([FromBody] Camisa camisa)
            {
                var nuevaCamisa = await Task.Run(() => camisaDB.Registrar(camisa));
                return Ok(nuevaCamisa);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> Actualizar(int id, [FromBody] Camisa camisa)
            {
                camisa.id_camisa = id;
                var camisaActualizada = await Task.Run(() => camisaDB.Actualizar(camisa));
                return Ok(camisaActualizada);
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Eliminar(int id)
            {
                var eliminado = await Task.Run(() => camisaDB.Eliminar(id));
                return Ok(eliminado);
            }
        
    }
}
