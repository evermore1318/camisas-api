using DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Controllers
{
    public class PagosController : Controller
    {
        private readonly IConfiguration _config;
        public PagosController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
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

        private Pago registrarPago(Pago pago)
        {
            Pago nuevoPago = null;
            using (var pagoHTTP = new HttpClient())
            {
                pagoHTTP.BaseAddress = new Uri(_config["Services:URL"]);
                StringContent contenido = new StringContent(JsonConvert.SerializeObject(pago),
                    System.Text.Encoding.UTF8, "application/json");
                var mensaje = pagoHTTP.PostAsync("Pagos", contenido).Result;
                var data = mensaje.Content.ReadAsStringAsync().Result;
                nuevoPago = JsonConvert.DeserializeObject<Pago>(data);
            }
            return nuevoPago;
        }
        public IActionResult RegistrarPago(int idPedido)
        {
            var pago = new Pago();
            pago.IdPedido = idPedido;

            var pedido = obtenerPorId(idPedido);
            ViewBag.DescripcionPedido = pedido.Descripcion;

            return View(pago);
        }

        [HttpPost]
        public IActionResult RegistrarPago(Pago pago)
        {
            if (ModelState.IsValid)
            {
                registrarPago(pago);
                return RedirectToAction("Index", "Pedidos");
            }
            return View(pago);
        }
    }
}
