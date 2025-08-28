using DSW_PROYECTO_PALACIO_CAMISAS_WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Models;

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
        using (var http = new HttpClient())
        {
            http.BaseAddress = new Uri(_config["Services:URL"]);
            var qs = $"camisas?marcaId={(filtro.MarcaId ?? 0)}&tipo={filtro.Tipo ?? ""}&talla={filtro.Talla ?? ""}&manga={filtro.Manga ?? ""}&color={filtro.Color ?? ""}";
            var resp = http.GetAsync(qs).Result;
            var data = resp.Content.ReadAsStringAsync().Result;
            resultado = string.IsNullOrWhiteSpace(data)
                ? new List<Camisa>()
                : JsonConvert.DeserializeObject<List<Camisa>>(data) ?? new List<Camisa>();
        }
        return resultado;
    }

    private Venta registrarVenta(Venta venta)
    {
        Venta creada = null;
        using (var http = new HttpClient())
        {
            http.BaseAddress = new Uri(_config["Services:URL"]);
            var contenido = new StringContent(JsonConvert.SerializeObject(venta), System.Text.Encoding.UTF8, "application/json");
            var resp = http.PostAsync("ventas", contenido).Result;
            var data = resp.Content.ReadAsStringAsync().Result;
            creada = string.IsNullOrWhiteSpace(data) ? null : JsonConvert.DeserializeObject<Venta>(data);
        }
        return creada;
    }

    private void CargarListasDesplegables()
    {
        // Marcas (útil para el modal de búsqueda)
        using (var http = new HttpClient())
        {
            http.BaseAddress = new Uri(_config["Services:URL"]);
            var resp = http.GetAsync("marcas").Result;
            var data = resp.Content.ReadAsStringAsync().Result;
            var marcas = string.IsNullOrWhiteSpace(data) ? new List<Marca>() : JsonConvert.DeserializeObject<List<Marca>>(data);
            ViewBag.Marcas = new SelectList(marcas ?? new List<Marca>(), "Id_Marca", "Descripcion");
        }
    }

    #endregion

    #region . ACCIONES MVC .

    // GET: /Ventas/Index
    [HttpGet]
    public IActionResult Index()
    {
        var ventas = obtenerVentas();

        // Si tu API aún no expone /ventas, puedes dejar un mock temporal:
        if (ventas == null || ventas.Count == 0)
        {
            ventas = new List<Venta>
                {
                    new Venta
                    {
                        id_venta = 1,
                        nombre_cliente = "Juan Pérez",
                        dni_cliente = "12345678",
                        tipo_pago = "Efectivo",
                        fecha = DateTime.Now,
                        detalles = new List<DetalleVenta>
                        {
                            new DetalleVenta{ Id_Camisa = 1, Cantidad = 2, Precio = 50 },
                            new DetalleVenta{ Id_Camisa = 2, Cantidad = 1, Precio = 70 }
                        }
                    },
                    new Venta
                    {
                        id_venta = 2,
                        nombre_cliente = "María Gómez",
                        dni_cliente = "87654321",
                        tipo_pago = "Tarjeta",
                        fecha = DateTime.Now.AddDays(-1),
                        detalles = new List<DetalleVenta>
                        {
                            new DetalleVenta{ Id_Camisa = 3, Cantidad = 3, Precio = 45 }
                        }
                    }
                };
        }

        return View(ventas);
    }

    // GET: /Ventas/Create
    [HttpGet]
    public IActionResult Create()
    {
        CargarListasDesplegables();
        var vm = new VentaCreate();
        return View(vm);
    }

    // POST: /Ventas/AgregarVenta (desde el form: action="Agregar")
    [HttpPost]
    public IActionResult AgregarVenta(VentaCreate vm)
    {
        if (vm.CamisaSeleccionadaId is null)
        {
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

        vm.Lineas.Add(new DetalleVentaTotal
        {
            Id_Camisa = camisa.id_camisa,
            Descripcion = camisa.descripcion,
            Presentacion = $"{camisa.color} / {camisa.talla} / {camisa.manga}",
            Cantidad = vm.Cantidad,
            PrecioUnitario = vm.PrecioUnitario > 0 ? vm.PrecioUnitario : camisa.precio_venta
        });

        // Limpiar selección
        vm.CamisaSeleccionadaId = null;
        vm.Cantidad = 1;
        vm.PrecioUnitario = 0;

        CargarListasDesplegables();
        return View("Create", vm);
    }

    // POST: /Ventas/QuitarVenta (desde el form: action="Quitar:{idx}")
    [HttpPost]
    public IActionResult QuitarVenta(VentaCreate venta, int idx)
    {
        if (idx >= 0 && idx < venta.Lineas.Count)
            venta.Lineas.RemoveAt(idx);

        CargarListasDesplegables();
        return View("Create", venta);
    }

    // GET: /Ventas/BuscarCamisas (usado por modal ajax)
    [HttpGet]
    public IActionResult BuscarCamisas([FromQuery] CamisaFiltro filtro)
    {
        var data = buscarCamisas(filtro);
        return PartialView("_BuscarCamisasPartial", data);
    }

    // POST: /Ventas/Create  (registrar venta en el back)
    [HttpPost]
    public IActionResult Create(VentaCreate venta, string? action)
    {
        // Manejo de acciones del formulario (Agregar/Quitar)
        if (action == "Agregar") return AgregarVenta(venta);
        if (action?.StartsWith("Quitar:") == true)
        {
            var idxStr = action.Split(':')[1];
            if (int.TryParse(idxStr, out var idx))
                return QuitarVenta(venta, idx);
        }

        if (!venta.Lineas.Any())
        {
            ModelState.AddModelError("", "Agrega al menos una camisa.");
            CargarListasDesplegables();
            return View(venta);
        }

        var dto = new Venta
        {
            nombre_cliente = venta.Nombre_Cliente,
            dni_cliente = venta.Dni_Cliente,
            tipo_pago = venta.Tipo_Pago,
            detalles = venta.Lineas.Select(l => new DetalleVenta
            {
                Id_Camisa = l.Id_Camisa,
                Cantidad = l.Cantidad,
                Precio = l.PrecioUnitario,
                Estado = "Activo"
            }).ToList()
        };

        var creada = registrarVenta(dto);
        if (creada != null)
        {
            TempData["msg"] = "Venta Registrada Satisfactoriamente. Por favor imprimir boleta.";
            return RedirectToAction(nameof(Comprobante), new { id = creada.id_venta });
        }

        ModelState.AddModelError("", "No se pudo registrar la venta.");
        CargarListasDesplegables();
        return View(venta);
    }

    // GET: /Ventas/Comprobante
    public IActionResult Comprobante(int? id = null)
    {
        ViewBag.IdVenta = id;
        return View();
    }

    #endregion
}

// ---- ViewModels auxiliares (si no están ya en tu proyecto) ----
public class CamisaFiltro
{
    public int? MarcaId { get; set; }
    public string? Tipo { get; set; }
    public string? Talla { get; set; }
    public string? Manga { get; set; }
    public string? Color { get; set; }

}