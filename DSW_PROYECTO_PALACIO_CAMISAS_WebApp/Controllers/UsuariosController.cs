using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IConfiguration _config;
        public UsuariosController(IConfiguration config)
        {
            _config = config;
        }

        #region . MÉTODOS PRIVADOS .

        private List<Usuario> obtenerUsuarios()
        {
            var listado = new List<Usuario>();
            using (var clienteHTTP = new HttpClient())
            {
                clienteHTTP.BaseAddress = new Uri(_config["Services:URL"]);
                var mensaje = clienteHTTP.GetAsync("Usuarios").Result;
                var data = mensaje.Content.ReadAsStringAsync().Result;
                listado = JsonConvert.DeserializeObject<List<Usuario>>(data);
            }
            return listado;
        }

        private Usuario obtenerPorId(int id)
        {
            Usuario usuario = null;
            using (var clienteHTTP = new HttpClient())
            {
                clienteHTTP.BaseAddress = new Uri(_config["Services:URL"]);
                var mensaje = clienteHTTP.GetAsync($"Usuarios/{id}").Result;
                var data = mensaje.Content.ReadAsStringAsync().Result;
                usuario = JsonConvert.DeserializeObject<Usuario>(data);
            }
            return usuario;
        }

        private List<Rol> obtenerRoles()
        {
            var roles = new List<Rol>();
            using (var clienteHTTP = new HttpClient())
            {
                clienteHTTP.BaseAddress = new Uri(_config["Services:URL"]);
                var mensaje = clienteHTTP.GetAsync("Usuarios/roles").Result;
                var data = mensaje.Content.ReadAsStringAsync().Result;
                roles = JsonConvert.DeserializeObject<List<Rol>>(data);
            }
            return roles;
        }

        private Usuario registrarUsuario(Usuario usuario)
        {
            Usuario nuevo = null;
            using (var clienteHTTP = new HttpClient())
            {
                clienteHTTP.BaseAddress = new Uri(_config["Services:URL"]);
                StringContent contenido = new StringContent(JsonConvert.SerializeObject(usuario),
                    System.Text.Encoding.UTF8, "application/json");
                var mensaje = clienteHTTP.PostAsync("Usuarios", contenido).Result;
                var data = mensaje.Content.ReadAsStringAsync().Result;
                nuevo = JsonConvert.DeserializeObject<Usuario>(data);
            }
            return nuevo;
        }

        private Usuario actualizarEstadoUsuario(int id, string nuevoEstado)
        {
            Usuario actualizado = null;
            using (var clienteHTTP = new HttpClient())
            {
                clienteHTTP.BaseAddress = new Uri(_config["Services:URL"]);
                var mensaje = clienteHTTP.PutAsync($"Usuarios/{id}/estado/{nuevoEstado}", null).Result;
                var data = mensaje.Content.ReadAsStringAsync().Result;
                actualizado = JsonConvert.DeserializeObject<Usuario>(data);
            }
            return actualizado;
        }

        private void eliminarUsuario(int id) 
        {
            using (var clienteHTTP = new HttpClient())
            {
                clienteHTTP.BaseAddress = new Uri(_config["Services:URL"]);
                var mensaje = clienteHTTP.DeleteAsync($"Usuarios/{id}").Result;
            }
        }

        #endregion

        public IActionResult Index()
        {
            var listado = obtenerUsuarios(); 
            return View(listado);
        }

        public IActionResult Create()
        {
            var roles = obtenerRoles();
            roles.Insert(0, new Rol() { IdRol = 0, Descripcion = "-- SELECCIONE --" });
            ViewBag.Roles = new SelectList(roles, "IdRol", "Descripcion");
            return View(new Usuario());
        }

        [HttpPost]
        public IActionResult Create(Usuario usuario)
        {
            registrarUsuario(usuario);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var usuario = obtenerPorId(id);
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Edit(int id, string estado)
        {
            actualizarEstadoUsuario(id, estado);
            return RedirectToAction("Details", new { id });
        }

        public IActionResult Details(int id)
        {
            var usuario = obtenerPorId(id);
            return View(usuario);
        }

        public IActionResult Delete(int id) 
        {
            var usuario = obtenerPorId(id);
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Delete(Usuario u) 
        {
            eliminarUsuario(u.IdUsuario);
            return RedirectToAction("Index");
        }
    }
}
