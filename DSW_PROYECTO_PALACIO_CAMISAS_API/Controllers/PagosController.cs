using DSW_PROYECTO_PALACIO_CAMISAS_API.Data;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly IPago pagoDB;

        public PagosController(IPago pagoRepo)
        {
            pagoDB = pagoRepo;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            return Ok(await Task.Run(() => pagoDB.ObtenerPorID(id)));
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(Pago pago)
        {
            return Ok(await Task.Run(() => pagoDB.Registrar(pago)));
        }
    
    }
}
