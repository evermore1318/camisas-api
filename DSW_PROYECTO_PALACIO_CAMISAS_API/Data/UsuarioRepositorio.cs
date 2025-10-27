using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data
{
    public class UsuarioRepositorio : IUsuario
    {
        // Datos en memoria TEMPORALES para Usuarios
        private List<Usuario> _usuarios = new List<Usuario>
        {
            new Usuario {
                IdUsuario = 1,
                Nombre = "admin",
                Password = "admin123",
                IdRol = 1,
                Estado = "Activo",
                RolDescripcion = "Administrador"
            },
            new Usuario {
                IdUsuario = 2,
                Nombre = "vendedor1",
                Password = "vendedor123",
                IdRol = 2,
                Estado = "Activo",
                RolDescripcion = "Vendedor"
            },
            new Usuario {
                IdUsuario = 3,
                Nombre = "inventario",
                Password = "inventario123",
                IdRol = 3,
                Estado = "Activo",
                RolDescripcion = "Inventario"
            },
            new Usuario {
                IdUsuario = 4,
                Nombre = "vendedor2",
                Password = "vendedor456",
                IdRol = 2,
                Estado = "Inactivo",
                RolDescripcion = "Vendedor"
            }
        };

        private List<Rol> _roles = new List<Rol>
        {
            new Rol { IdRol = 1, Descripcion = "Administrador", Estado = "Activo" },
            new Rol { IdRol = 2, Descripcion = "Vendedor", Estado = "Activo" },
            new Rol { IdRol = 3, Descripcion = "Inventario", Estado = "Activo" }
        };

        private int _nextId = 5;

        public List<Usuario> ListadoUsuarios()
        {
            return _usuarios.Select(u => new Usuario
            {
                IdUsuario = u.IdUsuario,
                Nombre = u.Nombre,
                IdRol = u.IdRol,
                RolDescripcion = u.RolDescripcion,
                Estado = u.Estado
            }).ToList();
        }

        public Usuario ObtenerPorID(int id)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.IdUsuario == id);
            if (usuario != null)
            {
                return new Usuario
                {
                    IdUsuario = usuario.IdUsuario,
                    Nombre = usuario.Nombre,
                    IdRol = usuario.IdRol,
                    RolDescripcion = usuario.RolDescripcion,
                    Estado = usuario.Estado
                };
            }
            return null;
        }

        public Usuario Registrar(Usuario usuario)
        {
            usuario.IdUsuario = _nextId++;
            usuario.Estado = "Activo";
            usuario.RolDescripcion = _roles.FirstOrDefault(r => r.IdRol == usuario.IdRol)?.Descripcion ?? "Usuario";
            _usuarios.Add(usuario);

            return new Usuario
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                IdRol = usuario.IdRol,
                RolDescripcion = usuario.RolDescripcion,
                Estado = usuario.Estado
            };
        }

        public Usuario ActualizarEstado(int id, string nuevoEstado)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.IdUsuario == id);
            if (usuario != null)
            {
                usuario.Estado = nuevoEstado;
                return new Usuario
                {
                    IdUsuario = usuario.IdUsuario,
                    Nombre = usuario.Nombre,
                    IdRol = usuario.IdRol,
                    RolDescripcion = usuario.RolDescripcion,
                    Estado = usuario.Estado
                };
            }
            return null;
        }

        public List<Rol> ListadoRoles()
        {
            return _roles.Where(r => r.Estado == "Activo").ToList();
        }

        public bool Eliminar(int id)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.IdUsuario == id);
            if (usuario != null)
            {
                _usuarios.Remove(usuario);
                return true;
            }
            return false;
        }
    }
}