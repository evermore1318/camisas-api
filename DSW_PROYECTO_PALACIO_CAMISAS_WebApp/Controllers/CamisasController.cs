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

        private List<Camisa> obtenerCamisas(CamisaFiltro filtro)
        {
            var listado = new List<Camisa>();
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri(_config["Services:URL"]);
                // Construir query preservando nulos/vacíos
                var qs = $"camisas?marcaId={(filtro.MarcaId ?? 0)}&tipo={filtro.Tipo ?? ""}&talla={filtro.Talla ?? ""}&manga={filtro.Manga ?? ""}&color={filtro.Color ?? ""}";
                var resp = http.GetAsync(qs).Result;
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
                // Nota: Ajusta los nombres de propiedades según tu modelo real
                ViewBag.Marcas = new SelectList(marcas ?? new List<Marca>(), "Id_Marca", "Descripcion");
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
