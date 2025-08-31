using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Models;
using DSW_PROYECTO_PALACIO_CAMISAS_WebApp.ViewModels;

namespace DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Controllers
{
    public class CamisasController : Controller
    {
        private readonly IConfiguration _config;
        public CamisasController(IConfiguration config)
        {
            _config = config;
        }

        #region . MÉTODOS PRIVADOS (HTTP HELPERS) .

        //sirve para armar la query solo con filtros presentes
        private static string CrearQuery(string path, CamisaFiltro filtro)
        {
            var parts = new List<string>();
            if (filtro != null)
            {
                if (filtro.MarcaId.HasValue) parts.Add($"marcaId={filtro.MarcaId.Value}");
                if (!string.IsNullOrWhiteSpace(filtro.Tipo)) parts.Add($"tipo={Uri.EscapeDataString(filtro.Tipo)}");
                if (!string.IsNullOrWhiteSpace(filtro.Talla)) parts.Add($"talla={Uri.EscapeDataString(filtro.Talla)}");
                if (!string.IsNullOrWhiteSpace(filtro.Manga)) parts.Add($"manga={Uri.EscapeDataString(filtro.Manga)}");
                if (!string.IsNullOrWhiteSpace(filtro.Color)) parts.Add($"color={Uri.EscapeDataString(filtro.Color)}");
            }
            return parts.Count == 0 ? path : $"{path}?{string.Join("&", parts)}";
        }
        private List<Camisa> obtenerCamisas(CamisaFiltro filtro)
        {
            var listado = new List<Camisa>();
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri(_config["Services:URL"]);
                // se modifica : usar crearquery en lugar del QS fijo con marcaId=0
                var resp = http.GetAsync(CrearQuery("camisas", filtro)).Result;
                var data = resp.Content.ReadAsStringAsync().Result;
                listado = string.IsNullOrWhiteSpace(data)
                    ? new List<Camisa>()
                    : JsonConvert.DeserializeObject<List<Camisa>>(data) ?? new List<Camisa>();
            }
            return listado;
        }

        private Camisa obtenerPorId(int id)
        {
            Camisa camisa = null;
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri(_config["Services:URL"]);
                var resp = http.GetAsync($"camisas/{id}").Result;
                var data = resp.Content.ReadAsStringAsync().Result;
                camisa = string.IsNullOrWhiteSpace(data) ? null : JsonConvert.DeserializeObject<Camisa>(data);
            }
            return camisa;
        }

        private Camisa registrarCamisa(Camisa camisa)
        {
            Camisa creada = null;
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri(_config["Services:URL"]);
                var contenido = new StringContent(JsonConvert.SerializeObject(camisa), System.Text.Encoding.UTF8, "application/json");
                var resp = http.PostAsync("camisas", contenido).Result;
                var data = resp.Content.ReadAsStringAsync().Result;
                // si el API devuelve la entidad creada
                creada = string.IsNullOrWhiteSpace(data) ? null : JsonConvert.DeserializeObject<Camisa>(data);
            }
            return creada;
        }

        private Camisa actualizarCamisa(int id, Camisa camisa)
        {
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri(_config["Services:URL"]);
                var contenido = new StringContent(JsonConvert.SerializeObject(camisa), System.Text.Encoding.UTF8, "application/json");
                var resp = http.PutAsync($"camisas/{id}", contenido).Result;
                var data = resp.Content.ReadAsStringAsync().Result;
                camisa = string.IsNullOrWhiteSpace(data) ? camisa : JsonConvert.DeserializeObject<Camisa>(data);
            }
            return camisa;
        }

        private bool eliminarCamisa(int id)
        {
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri(_config["Services:URL"]);
                var resp = http.DeleteAsync($"camisas/{id}").Result;
                return resp.IsSuccessStatusCode;
            }
        }

        private void CargarListasDesplegables()
        {
            // Marcas
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri(_config["Services:URL"]);
                var resp = http.GetAsync("marcas").Result;
                var data = resp.Content.ReadAsStringAsync().Result;
                var marcas = string.IsNullOrWhiteSpace(data) ? new List<Marca>() : JsonConvert.DeserializeObject<List<Marca>>(data);
                ViewBag.Marcas = new SelectList(marcas ?? new List<Marca>(), "id_marca", "descripcion");
            }
            // Si luego expones catálogos de Talla/Tipo/Manga/Color desde tu API,
            // aquí puedes cargarlos similar a Marcas y enviarlos por ViewBag.*
        }

        #endregion

        #region . ACCIONES MVC .

        // GET: /Camisas
        public IActionResult Index([FromQuery] CamisaFiltro filtro, int page = 1)
        {
            var listado = obtenerCamisas(filtro);

            // Paginación simple (15 por página)
            const int registrosPorPagina = 15;
            int totalRegistros = listado.Count;
            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);
            int omitir = registrosPorPagina * (page - 1);

            // Combos para filtros
            CargarListasDesplegables();

            // Mantener selección actual en filtros
            ViewBag.MarcaSeleccionada = filtro.MarcaId;
            ViewBag.Filtro = filtro;

            ViewBag.totalPaginas = totalPaginas;
            ViewBag.paginaActual = page;

            return View(listado.Skip(omitir).Take(registrosPorPagina).ToList());
        }

        // GET: /Camisas/Create
        public IActionResult Create()
        {
            CargarListasDesplegables();
            return View(new Camisa());
        }

        // POST: /Camisas/Create
        [HttpPost]
        public IActionResult Create(Camisa camisa)
        {
            if (!ModelState.IsValid)
            {
                CargarListasDesplegables();
                return View(camisa);
            }

            var creada = registrarCamisa(camisa);
            if (creada == null)
            {
                TempData["Mensaje"] = "No se pudo crear la camisa";
                TempData["TipoMensaje"] = "error";
                CargarListasDesplegables();
                return View(camisa);
            }

            TempData["Mensaje"] = "Camisa creada correctamente";
            TempData["TipoMensaje"] = "success";
            return RedirectToAction("Index");
        }

        // GET: /Camisas/Edit/5
        public IActionResult Edit(int id)
        {
            var camisa = obtenerPorId(id);
            if (camisa == null) return NotFound();

            CargarListasDesplegables();
            return View(camisa);
        }

        // POST: /Camisas/Edit/5
        [HttpPost]
        public IActionResult Edit(int id, Camisa camisa)
        {
            if (id != camisa.id_camisa) return BadRequest();

            if (!ModelState.IsValid)
            {
                CargarListasDesplegables();
                return View(camisa);
            }

            var actualizada = actualizarCamisa(id, camisa);
            TempData["Mensaje"] = "Camisa actualizada";
            TempData["TipoMensaje"] = "success";
            return RedirectToAction("Details", new { id = id });
        }

        // GET: /Camisas/Details/5
        public IActionResult Details(int id)
        {
            var camisa = obtenerPorId(id);
            if (camisa == null) return NotFound();
            return View(camisa);
        }

        // POST/GET: /Camisas/Delete/5 (usa GET para simplificar como tu ejemplo)
        public IActionResult Delete(int id)
        {
            bool ok = eliminarCamisa(id);
            TempData["Mensaje"] = ok ? "Camisa eliminada" : "No se pudo eliminar";
            TempData["TipoMensaje"] = ok ? "success" : "error";
            return RedirectToAction("Index");
        }

        // GET: /Camisas/Buscar?marcaId=&tipo=&talla=&manga=&color
        [HttpGet]
        public IActionResult Buscar([FromQuery] CamisaFiltro filtro)
        {
            var data = obtenerCamisas(filtro);
            return PartialView("BuscarCamisasPartial", data);
        }
        #endregion
    }

    // ---- ViewModel de filtros (ajusta al tuyo real si ya lo tienes en otra ruta) ----
    public class CamisaFiltro
    {
        public int? MarcaId { get; set; }
        public string? Tipo { get; set; }
        public string? Talla { get; set; }
        public string? Manga { get; set; }
        public string? Color { get; set; }
    }
}
