using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;
using Microsoft.Data.SqlClient;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data
{
    public class CamisaRepositorio : ICamisa
    {
        private readonly string cadenaConexion;
        private readonly IConfiguration _config;

        public CamisaRepositorio(IConfiguration config)
        {
            _config = config;
            cadenaConexion = _config["ConnectionStrings:DB"];
        }

        public List<Camisa> Listado()
        {
            var listado = new List<Camisa>();
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("ListarCamisas", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listado.Add(ConvertirReaderEnObjeto(reader));
                        }
                    }
                }
            }
            return listado;
        }

        public Camisa ObtenerPorID(int id)
        {
            Camisa camisa = null;
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("ObtenerCamisaPorID", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@id_camisa", id);
                    using (var reader = comando.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            reader.Read();
                            camisa = ConvertirReaderEnObjeto(reader);
                        }
                    }
                }
            }
            return camisa;
        }

        public Camisa Registrar(Camisa camisa)
        {
            Camisa nuevaCamisa = null;
            int nuevoId = 0;

            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("RegistrarCamisa", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@descripcion", camisa.descripcion);
                    comando.Parameters.AddWithValue("@id_marca", camisa.id_marca);
                    comando.Parameters.AddWithValue("@color", camisa.color);
                    comando.Parameters.AddWithValue("@talla", camisa.talla);
                    comando.Parameters.AddWithValue("@manga", camisa.manga);
                    comando.Parameters.AddWithValue("@stock", camisa.stock);
                    comando.Parameters.AddWithValue("@precio_costo", camisa.precio_costo);
                    comando.Parameters.AddWithValue("@precio_venta", camisa.precio_venta);
                    comando.Parameters.AddWithValue("@id_estante", camisa.id_estante);
                    comando.Parameters.AddWithValue("@estado", camisa.estado);
                    nuevoId = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            nuevaCamisa = ObtenerPorID(nuevoId);
            return nuevaCamisa;
        }

        public Camisa Actualizar(Camisa camisa)
        {
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("ActualizarCamisa", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@id_camisa", camisa.id_camisa);
                    comando.Parameters.AddWithValue("@descripcion", camisa.descripcion);
                    comando.Parameters.AddWithValue("@id_marca", camisa.id_marca);
                    comando.Parameters.AddWithValue("@color", camisa.color);
                    comando.Parameters.AddWithValue("@talla", camisa.talla);
                    comando.Parameters.AddWithValue("@manga", camisa.manga);
                    comando.Parameters.AddWithValue("@stock", camisa.stock);
                    comando.Parameters.AddWithValue("@precio_costo", camisa.precio_costo);
                    comando.Parameters.AddWithValue("@precio_venta", camisa.precio_venta);
                    comando.Parameters.AddWithValue("@id_estante", camisa.id_estante);
                    comando.Parameters.AddWithValue("@estado", camisa.estado);
                    comando.ExecuteNonQuery();
                }
            }
            return camisa;
        }

        public bool Eliminar(int id)
        {
            var exito = false;
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("EliminarCamisa", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@id_camisa", id);
                    exito = comando.ExecuteNonQuery() > 0;
                }
            }
            return exito;
        }

        private Camisa ConvertirReaderEnObjeto(SqlDataReader reader)
        {
            return new Camisa()
            {
                id_camisa = reader.GetInt32(0),
                descripcion = reader.GetString(1),
                id_marca = reader.GetInt32(2),
                color = reader.GetString(3),
                talla = reader.GetString(4),
                manga = reader.GetString(5),
                stock = reader.GetInt32(6),
                precio_costo = reader.GetDecimal(7),
                precio_venta = reader.GetDecimal(8),
                id_estante = reader.GetInt32(9),
                estado = reader.GetString(10),
                marca_nombre = reader.IsDBNull(11) ? null : reader.GetString(11),
                estante_descripcion = reader.IsDBNull(12) ? null : reader.GetString(12)
            };
        }
    }
}
