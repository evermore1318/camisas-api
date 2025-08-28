using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using Microsoft.AspNetCore.Mvc;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstantesController : ControllerBase
    {
        private readonly IEstante estanteDB;

        public EstantesController(IEstante estanteRepo)
        {
            estanteDB = estanteRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            return Ok(await Task.Run(() => estanteDB.Listado()));
        }
    }
}
