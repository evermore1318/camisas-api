using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models.DTOs;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data
{
    public class AuthRepositorio : IAuth
    {
        // Datos en memoria TEMPORALES para Autenticación
        private List<UsuarioDTO> _usuariosAuth = new List<UsuarioDTO>
        {
            new UsuarioDTO { ID = 1, NombreUsuario = "admin", Rol = "Administrador" },
            new UsuarioDTO { ID = 2, NombreUsuario = "vendedor1", Rol = "Vendedor" },
            new UsuarioDTO { ID = 3, NombreUsuario = "inventario", Rol = "Inventario" }
        };

        // Credenciales válidas (usuario: clave)
        private Dictionary<string, string> _credenciales = new Dictionary<string, string>
        {
            { "admin", "admin123" },
            { "vendedor1", "vendedor123" },
            { "inventario", "inventario123" }
        };

        public UsuarioDTO Login(string usuario, string clave)
        {
            // Verificar credenciales
            if (_credenciales.ContainsKey(usuario) && _credenciales[usuario] == clave)
            {
                return _usuariosAuth.FirstOrDefault(u => u.NombreUsuario == usuario);
            }

            return null; // Login fallido
        }
    }
}