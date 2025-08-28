using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly IReportes reporteDB;

        public ReportesController(IReportes reporteRepo)
        {
            reporteDB = reporteRepo;
        }

        [HttpGet("diario")]
        public async Task<IActionResult> GetReporteDiario([FromQuery] DateTime fecha)
        {
            var data = await reporteDB.ObtenerReporteDiario(fecha);
            return Ok(data);
        }
    }
}
