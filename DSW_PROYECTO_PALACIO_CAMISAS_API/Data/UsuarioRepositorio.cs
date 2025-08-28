using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;
using Microsoft.Data.SqlClient;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data
{
    public class UsuarioRepositorio : IUsuario
    {
        private readonly string cadenaConexion;
        private readonly IConfiguration _config;

        public UsuarioRepositorio(IConfiguration config)
        {
            _config = config;
            cadenaConexion = _config["ConnectionStrings:DB"];
        }

        public List<Usuario> ListadoUsuarios()
        {
            var listado = new List<Usuario>();
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("ListarUsuarios", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                            listado.Add(ReaderToUsuarioSinPassword(lector));
                    }
                }
            }
            return listado;
        }

        public Usuario ObtenerPorID(int id)
        {
            Usuario usuario = null;
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("ObtenerUsuarioPorID", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@Id", id);
                    using (var lector = comando.ExecuteReader())
                    {
                        if (lector != null && lector.HasRows)
                        {
                            lector.Read();
                            usuario = ReaderToUsuarioSinPassword(lector);
                        }
                    }
                }
            }
            return usuario;
        }

        public Usuario Registrar(Usuario usuario)
        {

            int nuevoID = 0;
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("RegistrarUsuario", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    comando.Parameters.AddWithValue("@Password", usuario.Password);
                    comando.Parameters.AddWithValue("@IdRol", usuario.IdRol);
                    comando.Parameters.AddWithValue("@Estado", usuario.Estado);
                    nuevoID = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            return ObtenerPorID(nuevoID);
            
        }

        public Usuario ActualizarEstado(int id, string nuevoEstado)
        {
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("ActualizarEstadoUsuario", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@Id", id);
                    comando.Parameters.AddWithValue("@Estado", nuevoEstado);
                    comando.ExecuteNonQuery();
                }
            }
            return ObtenerPorID(id);
        }


        public List<Rol> ListadoRoles()
        {
            var roles = new List<Rol>();
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("ListarRoles", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            roles.Add(new Rol
                            {
                                IdRol = lector.GetInt32(lector.GetOrdinal("IdRol")),
                                Descripcion = lector.GetString(lector.GetOrdinal("Descripcion")),
                                Estado = lector.GetString(lector.GetOrdinal("Estado"))
                            });
                        }
                    }
                }
            }
            return roles;
        }

        public bool Eliminar(int id) 
        {
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("EliminarUsuario", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@Id", id);
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        // sin password
        private Usuario ReaderToUsuarioSinPassword(SqlDataReader reader)
        {
            return new Usuario
            {
                IdUsuario = reader.GetInt32(reader.GetOrdinal("IdUsuario")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                IdRol = reader.GetInt32(reader.GetOrdinal("IdRol")),
                RolDescripcion = reader.GetString(reader.GetOrdinal("RolDescripcion")),
                Estado = reader.GetString(reader.GetOrdinal("Estado"))
            };
        }
    }
}
