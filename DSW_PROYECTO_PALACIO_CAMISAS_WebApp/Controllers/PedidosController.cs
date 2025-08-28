using DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Controllers
{
    public class PedidosController : Controller
    {
        private readonly IConfiguration _config;
        public PedidosController(IConfiguration config)
        {
            _config = config;
        }

        #region . METODOS PRIVADOS .

        private List<Pedido> obtenerPedidos()
        {
            var listado = new List<Pedido>();
            using (var pedidoHTTP = new HttpClient())
            {
                pedidoHTTP.BaseAddress = new Uri(_config["Services:URL"]);
                var mensaje = pedidoHTTP.GetAsync("Pedidos").Result;
                var data = mensaje.Content.ReadAsStringAsync().Result;

                listado = JsonConvert.DeserializeObject<List<Pedido>>(data);
            }
            return listado;
        }

        private Pedido obtenerPorId(int id)
        {
            Pedido pedido = null;
            using (var pedidoHTTP = new HttpClient())
            {
                pedidoHTTP.BaseAddress = new Uri(_config["Services:URL"]);
                var mensaje = pedidoHTTP.GetAsync($"pedidos/{id}").Result;
                var data = mensaje.Content.ReadAsStringAsync().Result;
                pedido = JsonConvert.DeserializeObject<Pedido>(data);
            }
            return pedido;
        }

        private Pedido registrarPedido(Pedido pedido)
        {
            Pedido nuevoPedido = null;
            using (var pedidoHTTP = new HttpClient())
            {
                pedidoHTTP.BaseAddress = new Uri(_config["Services:URL"]);
                StringContent contenido = new StringContent(JsonConvert.SerializeObject(pedido),
                    System.Text.Encoding.UTF8, "application/json");
                var mensaje = pedidoHTTP.PostAsync("Pedidos", contenido).Result;
                var data = mensaje.Content.ReadAsStringAsync().Result;
                nuevoPedido = JsonConvert.DeserializeObject<Pedido>(data);
            }
            return nuevoPedido;
        }

        private Pedido actualizarPedido(Pedido pedido)
        {
            using (var pedidoHTTP = new HttpClient())
            {
                pedidoHTTP.BaseAddress = new Uri(_config["Services:URL"]);
                var contenido = new StringContent(JsonConvert.SerializeObject(pedido),
                    System.Text.Encoding.UTF8, "application/json");
                var mensaje = pedidoHTTP.PutAsync($"pedidos/{pedido.IdPedido}", contenido).Result;
                var data = mensaje.Content.ReadAsStringAsync().Result;
                pedido = JsonConvert.DeserializeObject<Pedido>(data);
            }
            return pedido;
        }

        private List<Proveedor> obtenerProveedores()
        {
            var listado = new List<Proveedor>();
            using (var proveedorHTTP = new HttpClient())
            {
                proveedorHTTP.BaseAddress = new Uri(_config["Services:URL"]);
                var mensaje = proveedorHTTP.GetAsync("Proveedores").Result;
                var data = mensaje.Content.ReadAsStringAsync().Result;
                listado = JsonConvert.DeserializeObject<List<Proveedor>>(data);
            }
            return listado;
        }

        #endregion

        public IActionResult Index(int page = 1, int numreg = 15, int proveedor = 0, int anio = 0, int mes = 0)
        {
            var listado = obtenerPedidos();

            if (proveedor > 0)
                listado = listado.Where(p => p.Proveedor != null && p.Proveedor.IdProveedor == proveedor).ToList();

            if (anio > 0)
                listado = listado.Where(p => p.Fecha.Year == anio).ToList();

            if (mes > 0)
                listado = listado.Where(p => p.Fecha.Month == mes).ToList();

            
            int totalRegistros = listado.Count();
            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / numreg);
            int omitir = numreg * (page - 1);

            var proveedores = obtenerProveedores();
            proveedores.Insert(0, new Proveedor { IdProveedor = 0, Nombre = "--Seleccione--" });
            ViewBag.proveedores = new SelectList(proveedores, "IdProveedor", "Nombre", proveedor);

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

            var años = new List<SelectListItem>();
            años.Add(new SelectListItem { Value = "0", Text = "--Seleccione--" }); // valor neutro
            for (int i = DateTime.Now.Year; i >= DateTime.Now.Year - 10; i--)
            {
                años.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
            }
            ViewBag.años = new SelectList(años, "Value", "Text", anio);

            ViewBag.totalPaginas = totalPaginas;
            ViewBag.paginaActual = page;
            ViewBag.numreg = numreg;
            ViewBag.proveedorSeleccionado = proveedor;
            ViewBag.anioSeleccionado = anio;
            ViewBag.mesSeleccionado = mes;

            return View(listado.Skip(omitir).Take(numreg).ToList());
        }

        public IActionResult Create()
        {
            var proveedores = obtenerProveedores();
            proveedores.Insert(0, new Proveedor { IdProveedor = 0, Nombre = "--Seleccione--" });

            ViewBag.Proveedores = new SelectList(proveedores, "IdProveedor", "Nombre");
            return View(new Pedido());
        }

        [HttpPost]
        public IActionResult Create(Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                registrarPedido(pedido);
                return RedirectToAction("Index");
            }

            var proveedores = obtenerProveedores();
            proveedores.Insert(0, new Proveedor { IdProveedor = 0, Nombre = "--Seleccione--" });
            ViewBag.Proveedores = new SelectList(proveedores, "IdProveedor", "Nombre", pedido.IdProveedor);

            return View(pedido);
        }

        public IActionResult Edit(int id)
        {
            var pedido = obtenerPorId(id);
            if (pedido == null)
            {
                return NotFound();
            }

            var proveedores = obtenerProveedores();
            ViewBag.Proveedores = new SelectList(proveedores, "IdProveedor", "Nombre", pedido.IdProveedor);

            return View(pedido);
        }

        [HttpPost]
        public IActionResult Edit(Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                actualizarPedido(pedido);
                return RedirectToAction("Index");
            }

            var proveedores = obtenerProveedores();
            ViewBag.Proveedores = new SelectList(proveedores, "IdProveedor", "Nombre", pedido.IdProveedor);

            return View(pedido);
        }

        public IActionResult Details(int id)
        {

            var pedido = obtenerPorId(id);
            if (pedido == null)
            {
                return NotFound();
            }

            if (pedido.IdProveedor != 0)
            {
                var proveedores = obtenerProveedores();
                pedido.Proveedor = proveedores.FirstOrDefault(p => p.IdProveedor == pedido.IdProveedor);
            }

            return View(pedido);
        }

    }
}
