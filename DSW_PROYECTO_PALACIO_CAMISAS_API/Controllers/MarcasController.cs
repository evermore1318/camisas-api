using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using Microsoft.AspNetCore.Mvc;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcasController : ControllerBase
    {
        private readonly IMarca marcaDB;

        public MarcasController(IMarca marcaRepo)
        {
            marcaDB = marcaRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            return Ok(await Task.Run(() => marcaDB.Listado()));
        }
    }
}
