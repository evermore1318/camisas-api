using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;
using Microsoft.Data.SqlClient;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data
{
    public class ProveedorRepositorio : IProveedor
    {
        private readonly string cadenaConexion;
        private readonly IConfiguration _config;
        public ProveedorRepositorio(IConfiguration config)
        {
            _config = config;
            cadenaConexion = config["ConnectionStrings:DB"];
        }

        public List<Proveedor> listado()
        {
            var listado = new List<Proveedor>();
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var command = new SqlCommand("ListarProveedores", conexion))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                listado.Add(new Proveedor()
                                {
                                    IdProveedor = reader.GetInt32(reader.GetOrdinal("id_proveedor")),
                                    IdMarca = reader.GetInt32(reader.GetOrdinal("id_marca")),
                                    Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                    Telefono = reader.GetString(reader.GetOrdinal("telefono")),
                                    Direccion = reader.GetString(reader.GetOrdinal("direccion")),
                                    Email = reader.GetString(reader.GetOrdinal("email")),
                                    Estado = reader.GetString(reader.GetOrdinal("estado"))
                                });
                            }
                        }
                    }
                }
            }
            return listado;
        }
    }
}
