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
                ViewBag.Mensaje = "Usuario o clave incorrectos.";
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
