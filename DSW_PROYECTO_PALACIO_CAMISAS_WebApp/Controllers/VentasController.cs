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
            var parts = new List<string>();
            if (filtro != null)
            {
                if (filtro.MarcaId.HasValue) parts.Add($"marcaId={filtro.MarcaId.Value}");
                if (!string.IsNullOrWhiteSpace(filtro.Tipo)) parts.Add($"tipo={Uri.EscapeDataString(filtro.Tipo)}");
                if (!string.IsNullOrWhiteSpace(filtro.Talla)) parts.Add($"talla={Uri.EscapeDataString(filtro.Talla)}");
                if (!string.IsNullOrWhiteSpace(filtro.Manga)) parts.Add($"manga={Uri.EscapeDataString(filtro.Manga)}");
                if (!string.IsNullOrWhiteSpace(filtro.Color)) parts.Add($"color={Uri.EscapeDataString(filtro.Color)}");
            }
            var url = "camisas" + (parts.Count > 0 ? "?" + string.Join("&", parts) : "");
            var resp = http.GetAsync(url).Result;
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
            ViewBag.Marcas = new SelectList(marcas ?? new List<Marca>(), "id_marca", "descripcion");
        }
    }
    private bool actualizarEstadoVenta(int id, string nuevoEstado)
    {
        using (var http = new HttpClient())
        {
            http.BaseAddress = new Uri(_config["Services:URL"]);
            var payload = new { estado = nuevoEstado }; // solo mandamos estado
            var content = new StringContent(
                JsonConvert.SerializeObject(payload),
                System.Text.Encoding.UTF8,
                "application/json"
            );
            var resp = http.PutAsync($"api/ventas/{id}", content).Result;
            return resp.IsSuccessStatusCode;
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
        
        //agregado para el listado de ventas
        var cache = new Dictionary<int, Camisa>();
        Camisa GetCamisa(int id)
        {
            if (!cache.TryGetValue(id, out var c))
            {
                c = obtenerCamisaPorId(id) ?? new Camisa { id_camisa = id, descripcion = "(no encontrada)" };
                cache[id] = c;
            }
            return c;
        }

        var lineasPorVenta = new Dictionary<int, List<DetalleVentaTotal>>();
        foreach (var v in ventas)
        {
            var lineas = new List<DetalleVentaTotal>();
            foreach (var d in v.detalles ?? new List<DetalleVenta>())
            {
                var c = GetCamisa(d.Id_Camisa);
                lineas.Add(new DetalleVentaTotal
                {
                    Id_Camisa = c.id_camisa,
                    Descripcion = c.descripcion,
                    Presentacion = $"{c.color} / {c.talla} / {c.manga}",
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.Precio
                });
            }
            lineasPorVenta[v.id_venta] = lineas;
        }
        ViewBag.LineasPorVenta = lineasPorVenta; // para listar boletas eb vista venta

        foreach (var v in ventas) //para poblar correctamente todos los detalles
        {
            v.DetallesTotal = new List<DetalleVentaTotal>();

            foreach (var d in v.detalles)
            {
                var camisa = obtenerCamisaPorId(d.Id_Camisa);
                if (camisa != null)
                {
                    v.DetallesTotal.Add(new DetalleVentaTotal
                    {
                        Id_Camisa = camisa.id_camisa,
                        Descripcion = camisa.descripcion,
                        Presentacion = $"{camisa.color} / {camisa.talla} / {camisa.manga}",
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.Precio
                    });
                }
            }

            // recalcular el total en base a los subtotales
            v.precio_total = v.DetallesTotal.Sum(x => x.Subtotal);
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
        return PartialView("BuscarCamisasPartial.cshtml", data);
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

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var venta = obtenerVentas().FirstOrDefault(v => v.id_venta == id);
        if (venta == null) return NotFound();

        ViewBag.Estados = new List<string> { "Activo", "Anulado", "Pendiente" };
        return View(venta);
    }

    //  POST /Ventas/Edit/{id} -> envía solo el estado al API
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, string estado)
    {
        if (string.IsNullOrWhiteSpace(estado))
        {
            ModelState.AddModelError("estado", "Selecciona un estado.");
            var ventaInvalid = obtenerVentas().FirstOrDefault(v => v.id_venta == id) ?? new Venta { id_venta = id };
            ViewBag.Estados = new List<string> { "Activo", "Anulado", "Pendiente" };
            return View(ventaInvalid);
        }

        var ok = actualizarEstadoVenta(id, estado);
        if (ok)
        {
            TempData["msg"] = "Estado actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", "No se pudo actualizar el estado en el servicio.");
        var venta = obtenerVentas().FirstOrDefault(v => v.id_venta == id) ?? new Venta { id_venta = id };
        ViewBag.Estados = new List<string> { "Activo", "Anulado", "Pendiente" };
        return View(venta);
    }

    #endregion
}