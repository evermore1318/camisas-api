using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Models;
using DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Models.DTOs;

namespace DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        #region . METODOS PRIVADOS .
        private UsuarioDTO autenticarUsuario(LoginRequest request)
        {
            UsuarioDTO user = null;

            using (var clienteHTTP = new HttpClient())
            {
                clienteHTTP.BaseAddress = new Uri(_config["Services:URL"]);

                var respuesta = clienteHTTP.PostAsJsonAsync("Auth/login", request).Result;

                if (respuesta.IsSuccessStatusCode)
                {
                    var data = respuesta.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<UsuarioDTO>(data);
                }
            }

            return user;
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string usuario, string clave)
        {
            var request = new LoginRequest { Usuario = usuario, Clave = clave };
            var user = autenticarUsuario(request);

            if (user == null)
            {
                ViewBag.Mensaje = "Credenciales inválidas.";
                return View();
            }

            HttpContext.Session.SetString("Usuario", user.NombreUsuario);
            HttpContext.Session.SetString("Rol", user.Rol);
            HttpContext.Session.SetInt32("IdUsuario", user.ID);

            return RedirectToAction("Inicio");
        }

        public IActionResult Inicio()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario")))
            {
                return RedirectToAction("Index", "Login");
            }

            try
            {
                using (var http = new HttpClient())
                {
                    http.BaseAddress = new Uri(_config["Services:URL"]);

                    // Camisas
                    var resp = http.GetAsync("camisas").Result;
                    var data = resp.Content.ReadAsStringAsync().Result;
                    var camisas = JsonConvert.DeserializeObject<List<Camisa>>(data);
                    ViewBag.TotalCamisas = camisas?.Count ?? 0;

                    // Usuarios
                    resp = http.GetAsync("usuarios").Result;
                    data = resp.Content.ReadAsStringAsync().Result;
                    var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(data);
                    ViewBag.TotalUsuarios = usuarios?.Count ?? 0;

                    // Ventas
                    resp = http.GetAsync("ventas").Result;
                    data = resp.Content.ReadAsStringAsync().Result;
                    var ventas = JsonConvert.DeserializeObject<List<Venta>>(data);
                    ViewBag.TotalVentas = ventas?.Count ?? 0;
                }
            }
            catch
            {
                ViewBag.TotalCamisas = 0;
                ViewBag.TotalUsuarios = 0;
                ViewBag.TotalVentas = 0;
            }

            ViewData["Title"] = "Dashboard";
            ViewData["Subtitle"] = "Panel de control principal";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete(".AspNetCore.Session");
            return RedirectToAction("Index", "Login");
        }
    }
}
