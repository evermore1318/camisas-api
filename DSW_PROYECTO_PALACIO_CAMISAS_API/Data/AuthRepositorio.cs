using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models.DTOs;
using Microsoft.Data.SqlClient;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data
{
    public class AuthRepositorio : IAuth
    {
        private readonly string cadenaConexion;
        private readonly IConfiguration _config;

        public AuthRepositorio(IConfiguration config)
        {
            _config = config;
            cadenaConexion = _config["ConnectionStrings:DB"];
        }

        public UsuarioDTO Login(string usuario, string clave)
        {
            UsuarioDTO user = null;

            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("LoginUsuario", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@Usuario", usuario);
                    comando.Parameters.AddWithValue("@Clave", clave);

                    using (var lector = comando.ExecuteReader())
                    {
                        if (lector != null && lector.HasRows)
                        {
                            lector.Read();
                            user = ConvertirReaderEnObjeto(lector);
                        }
                    }
                }
            }

            return user;
        }

        #region . METODO PRIVADO .
        private UsuarioDTO ConvertirReaderEnObjeto(SqlDataReader reader)
        {
            return new UsuarioDTO()
            {
                ID = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                NombreUsuario = reader.GetString(reader.GetOrdinal("nombre")),
                Rol = reader.GetString(reader.GetOrdinal("descripcion"))
            };
        }
        #endregion
    }
}

