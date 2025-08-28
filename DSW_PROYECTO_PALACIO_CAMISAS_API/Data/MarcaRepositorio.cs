using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;
using Microsoft.Data.SqlClient;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data
{
    public class MarcaRepositorio : IMarca
    {
        private readonly string cadenaConexion;
        private readonly IConfiguration _config;

        public MarcaRepositorio(IConfiguration config)
        {
            _config = config;
            cadenaConexion = _config["ConnectionStrings:DB"];
        }

        public List<Marca> Listado()
        {
            var listado = new List<Marca>();
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("ListarMarcas", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listado.Add(new Marca()
                            {
                                id_marca = reader.GetInt32(0),
                                descripcion = reader.GetString(1),
                                estado = reader.GetString(2)
                            });
                        }
                    }
                }
            }
            return listado;
        }
    }
}
