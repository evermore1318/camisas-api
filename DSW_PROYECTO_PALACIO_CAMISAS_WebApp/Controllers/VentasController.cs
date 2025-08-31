using DSW_PROYECTO_PALACIO_CAMISAS_WebApp.ViewModels;
using DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Models.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Models;

namespace DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Controllers
{
    public class VentasController : Controller
    {
        private readonly IConfiguration _config;
        public VentasController(IConfiguration config)
        {
            _config = config;
        }

        #region . MÉTODOS PRIVADOS (HTTP HELPERS) .

        private List<Venta> obtenerVentas()
        {
            var listado = new List<Venta>();
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri(_config["Services:URL"]);
                var resp = http.GetAsync("ventas").Result;
                var data = resp.Content.ReadAsStringAsync().Result;
                listado = string.IsNullOrWhiteSpace(data)
                    ? new List<Venta>()
                    : JsonConvert.DeserializeObject<List<Venta>>(data) ?? new List<Venta>();
            }
            return listado;
        }

        private Venta obtenerVentaPorId(int id)
        {
            Venta venta = null;
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri(_config["Services:URL"]);
                var resp = http.GetAsync($"ventas/{id}").Result;
                var data = resp.Content.ReadAsStringAsync().Result;
                venta = string.IsNullOrWhiteSpace(data) ? null : JsonConvert.DeserializeObject<Venta>(data);
            }
            return venta;
        }

        private Camisa obtenerCamisaPorId(int id)
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

        private List<Camisa> buscarCamisas(CamisaFiltro filtro)
        {
            var resultado = new List<Camisa>();

            try
            {
                using (var http = new HttpClient())
                {
                    http.BaseAddress = new Uri(_config["Services:URL"]);

                    // Construir query string igual que el CamisasController MVC
                    var qs = $"camisas?marcaId={filtro?.MarcaId ?? 0}&tipo={Uri.EscapeDataString(filtro?.Tipo ?? "")}&talla={Uri.EscapeDataString(filtro?.Talla ?? "")}&manga={Uri.EscapeDataString(filtro?.Manga ?? "")}&color={Uri.EscapeDataString(filtro?.Color ?? "")}";

                    System.Diagnostics.Debug.WriteLine($"URL completa: {http.BaseAddress}{qs}");

                    var resp = http.GetAsync(qs).Result;

                    if (resp.IsSuccessStatusCode)
                    {
                        var data = resp.Content.ReadAsStringAsync().Result;
                        System.Diagnostics.Debug.WriteLine($"Datos recibidos: {data.Substring(0, Math.Min(500, data.Length))}...");

                        resultado = string.IsNullOrWhiteSpace(data)
                            ? new List<Camisa>()
                            : JsonConvert.DeserializeObject<List<Camisa>>(data) ?? new List<Camisa>();

                        System.Diagnostics.Debug.WriteLine($"Camisas deserializadas: {resultado.Count}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Error HTTP: {resp.StatusCode} - {resp.ReasonPhrase}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Excepción en buscarCamisas: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }

            return resultado;
        }

        private Venta registrarVenta(VentaCreateDto ventaDto)
        {
            Venta creada = null;
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri(_config["Services:URL"]);
                var contenido = new StringContent(JsonConvert.SerializeObject(ventaDto), System.Text.Encoding.UTF8, "application/json");
                var resp = http.PostAsync("ventas", contenido).Result;
                var data = resp.Content.ReadAsStringAsync().Result;
                creada = string.IsNullOrWhiteSpace(data) ? null : JsonConvert.DeserializeObject<Venta>(data);
            }
            return creada;
        }

        private bool anularVenta(int id)
        {
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri(_config["Services:URL"]);
                var updateDto = new { estado = "Anulado" };
                var contenido = new StringContent(JsonConvert.SerializeObject(updateDto), System.Text.Encoding.UTF8, "application/json");
                var resp = http.PutAsync($"ventas/{id}", contenido).Result;
                return resp.IsSuccessStatusCode;
            }
        }

        private void CargarListasDesplegables()
        {
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri(_config["Services:URL"]);
                var resp = http.GetAsync("marcas").Result;
                var data = resp.Content.ReadAsStringAsync().Result;
                var marcas = string.IsNullOrWhiteSpace(data) ? new List<Marca>() : JsonConvert.DeserializeObject<List<Marca>>(data);
                ViewBag.Marcas = new SelectList(marcas ?? new List<Marca>(), "id_marca", "descripcion");
            }
        }

        #endregion

        #region . ACCIONES MVC .

        // GET: /Ventas/Index
        [HttpGet]
        public IActionResult Index()
        {
            var ventas = obtenerVentas();
            return View(ventas);
        }

        // GET: /Ventas/Details/5
        public IActionResult Details(int id)
        {
            var venta = obtenerVentaPorId(id);
            if (venta == null)
                return NotFound();
            return View(venta);
        }

        // GET: /Ventas/Create
        [HttpGet]
        public IActionResult Create()
        {
            CargarListasDesplegables();
            var vm = new VentaCreate();
            return View(vm);
        }

        // POST: /Ventas/AgregarVenta
        [HttpPost]
        public IActionResult AgregarVenta(VentaCreate vm)
        {
            if (!vm.CamisaSeleccionadaId.HasValue)
            {
                ModelState.AddModelError("", "Seleccione una camisa");
                CargarListasDesplegables();
                return View("Create", vm);
            }

            var camisa = obtenerCamisaPorId(vm.CamisaSeleccionadaId.Value);
            if (camisa == null)
            {
                ModelState.AddModelError("", "Camisa no encontrada");
                CargarListasDesplegables();
                return View("Create", vm);
            }

            // Verificar si ya existe la camisa en las líneas
            var lineaExistente = vm.Lineas.FirstOrDefault(l => l.Id_Camisa == camisa.id_camisa);
            if (lineaExistente != null)
            {
                // Actualizar cantidad existente
                lineaExistente.Cantidad += vm.Cantidad;
                if (vm.PrecioUnitario > 0)
                    lineaExistente.PrecioUnitario = vm.PrecioUnitario;
            }
            else
            {
                // Agregar nueva línea
                vm.Lineas.Add(new DetalleVentaTotal
                {
                    Id_Camisa = camisa.id_camisa,
                    Descripcion = camisa.descripcion,
                    Presentacion = $"{camisa.marca_nombre} - {camisa.color} - {camisa.talla} - {camisa.manga}",
                    Cantidad = vm.Cantidad,
                    PrecioUnitario = vm.PrecioUnitario > 0 ? vm.PrecioUnitario : camisa.precio_venta
                });
            }

            // Limpiar selección
            vm.CamisaSeleccionadaId = null;
            vm.Cantidad = 1;
            vm.PrecioUnitario = 0;

            CargarListasDesplegables();
            return View("Create", vm);
        }

        // POST: /Ventas/QuitarVenta
        [HttpPost]
        public IActionResult QuitarVenta(VentaCreate venta, int idx)
        {
            if (idx >= 0 && idx < venta.Lineas.Count)
                venta.Lineas.RemoveAt(idx);

            CargarListasDesplegables();
            return View("Create", venta);
        }

        // GET: /Ventas/BuscarCamisas
        [HttpGet]
        public IActionResult BuscarCamisas([FromQuery] CamisaFiltro filtro)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== INICIO BuscarCamisas ===");
                System.Diagnostics.Debug.WriteLine($"Filtros - MarcaId: {filtro?.MarcaId}, Tipo: '{filtro?.Tipo}', Talla: '{filtro?.Talla}', Manga: '{filtro?.Manga}', Color: '{filtro?.Color}'");

                var data = buscarCamisas(filtro ?? new CamisaFiltro());

                System.Diagnostics.Debug.WriteLine($"Camisas encontradas: {data.Count}");

                // Log de algunas camisas encontradas para debug
                foreach (var camisa in data.Take(3))
                {
                    System.Diagnostics.Debug.WriteLine($"  - ID: {camisa.id_camisa}, Desc: {camisa.descripcion}, Marca: {camisa.marca_nombre}, Stock: {camisa.stock}");
                }

                System.Diagnostics.Debug.WriteLine("=== FIN BuscarCamisas ===");

                return PartialView("_BuscarCamisasPartial", data);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR en BuscarCamisas: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                // En caso de error, retornar lista vacía
                return PartialView("_BuscarCamisasPartial", new List<Camisa>());
            }
        }

        // POST: /Ventas/Create
        [HttpPost]
        public IActionResult Create(VentaCreate venta, string? action)
        {
            // Manejo de acciones del formulario
            if (action == "Agregar")
                return AgregarVenta(venta);

            if (action?.StartsWith("Quitar:") == true)
            {
                var idxStr = action.Split(':')[1];
                if (int.TryParse(idxStr, out var idx))
                    return QuitarVenta(venta, idx);
            }

            // Validación antes de registrar
            if (!venta.Lineas.Any())
            {
                ModelState.AddModelError("", "Agrega al menos una camisa.");
                CargarListasDesplegables();
                return View(venta);
            }

            if (string.IsNullOrWhiteSpace(venta.Nombre_Cliente) || string.IsNullOrWhiteSpace(venta.Dni_Cliente))
            {
                ModelState.AddModelError("", "Complete los datos del cliente.");
                CargarListasDesplegables();
                return View(venta);
            }

            // Crear DTO para enviar al API
            var ventaDto = new VentaCreateDto
            {
                nombre_cliente = venta.Nombre_Cliente,
                dni_cliente = venta.Dni_Cliente,
                tipo_pago = venta.Tipo_Pago,
                detalles = venta.Lineas.Select(l => new DetalleVentaCreateDto
                {
                    id_camisa = l.Id_Camisa,
                    cantidad = l.Cantidad,
                    precio = l.PrecioUnitario
                }).ToList()
            };

            var creada = registrarVenta(ventaDto);
            if (creada != null)
            {
                TempData["msg"] = "Venta registrada satisfactoriamente.";
                TempData["tipo"] = "success";
                return RedirectToAction(nameof(Comprobante), new { id = creada.id_venta });
            }

            ModelState.AddModelError("", "No se pudo registrar la venta.");
            CargarListasDesplegables();
            return View(venta);
        }

        // POST: /Ventas/Anular/5
        [HttpPost]
        public IActionResult Anular(int id)
        {
            var exito = anularVenta(id);
            if (exito)
            {
                TempData["msg"] = "Venta anulada correctamente.";
                TempData["tipo"] = "success";
            }
            else
            {
                TempData["msg"] = "No se pudo anular la venta.";
                TempData["tipo"] = "error";
            }

            return RedirectToAction("Index");
        }

        // GET: /Ventas/Comprobante/5
        public IActionResult Comprobante(int id)
        {
            var venta = obtenerVentaPorId(id);
            if (venta == null)
                return NotFound();

            return View(venta);
        }

        #endregion
    }
}