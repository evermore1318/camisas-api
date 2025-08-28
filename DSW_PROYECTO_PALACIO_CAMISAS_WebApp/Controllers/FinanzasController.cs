using DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Controllers
{
    public class FinanzasController : Controller
    {
        private readonly IConfiguration _config;

        public FinanzasController(IConfiguration config)
        {
            _config = config;
        }

        private FinanzasDto obtenerResumen(int anio, int mes)
        {
            FinanzasDto resumen = null;
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri(_config["Services:URL"]);
                var mensaje = http.GetAsync($"finanzas/resumen/{anio}/{mes}").Result;
                var data = mensaje.Content.ReadAsStringAsync().Result;
                resumen = JsonConvert.DeserializeObject<FinanzasDto>(data);
            }
            return resumen;
        }

        public IActionResult Index(int anio = 0, int mes = 0)
        {
            var meses = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "--Seleccione--" },
                new SelectListItem { Value = "1", Text = "Enero" },
                new SelectListItem { Value = "2", Text = "Febrero" },
                new SelectListItem { Value = "3", Text = "Marzo" },
                new SelectListItem { Value = "4", Text = "Abril" },
                new SelectListItem { Value = "5", Text = "Mayo" },
                new SelectListItem { Value = "6", Text = "Junio" },
                new SelectListItem { Value = "7", Text = "Julio" },
                new SelectListItem { Value = "8", Text = "Agosto" },
                new SelectListItem { Value = "9", Text = "Septiembre" },
                new SelectListItem { Value = "10", Text = "Octubre" },
                new SelectListItem { Value = "11", Text = "Noviembre" },
                new SelectListItem { Value = "12", Text = "Diciembre" },
            };

            ViewBag.meses = new SelectList(meses, "Value", "Text", mes);
            ViewBag.Mes = mes;
            ViewBag.Anio = anio;

            FinanzasDto resumen = null;

            if (anio > 0 && mes > 0)
            {
                using (var http = new HttpClient())
                {
                    http.BaseAddress = new Uri(_config["Services:URL"]);
                    var response = http.GetAsync($"Finanzas/resumen/{anio}/{mes}").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        resumen = JsonConvert.DeserializeObject<FinanzasDto>(data);
                    }
                }
            }

            return View(resumen);
        }
    }
}
