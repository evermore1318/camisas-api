using DSW_PROYECTO_PALACIO_CAMISAS_API.Data;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanzasController : ControllerBase
    {
        private readonly IFinanzas finanzasDB;

        public FinanzasController(IFinanzas finanzasRepo)
        {
            finanzasDB = finanzasRepo;
        }

        // 1. Ingresos Mensuales
        [HttpGet("ingresos/mensual/{anio}/{mes}")]
        public async Task<IActionResult> ObtenerIngresosMensuales(int anio, int mes)
        {
            return Ok(await Task.Run(() => finanzasDB.ObtenerIngresosMensuales(anio, mes)));
        }

        // 2. Ingresos Anuales
        [HttpGet("ingresos/anual/{anio}")]
        public async Task<IActionResult> ObtenerIngresosAnuales(int anio)
        {
            return Ok(await Task.Run(() => finanzasDB.ObtenerIngresosAnuales(anio)));
        }

        // 3. Egresos Mensuales
        [HttpGet("egresos/mensual/{anio}/{mes}")]
        public async Task<IActionResult> ObtenerEgresosMensuales(int anio, int mes)
        {
            return Ok(await Task.Run(() => finanzasDB.ObtenerEgresosMensuales(anio, mes)));
        }

        // 4. Egresos Anuales
        [HttpGet("egresos/anual/{anio}")]
        public async Task<IActionResult> ObtenerEgresosAnuales(int anio)
        {
            return Ok(await Task.Run(() => finanzasDB.ObtenerEgresosAnuales(anio)));
        }

        // 5. Resumen (los 4 valores juntos en un DTO)
        [HttpGet("resumen/{anio}/{mes}")]
        public async Task<IActionResult> ObtenerResumen(int anio, int mes)
        {
            return Ok(await Task.Run(() => finanzasDB.ObtenerResumen(anio, mes)));
        }

    }
}
