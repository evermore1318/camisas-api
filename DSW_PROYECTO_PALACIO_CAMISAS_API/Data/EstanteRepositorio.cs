using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;
using Microsoft.Data.SqlClient;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data
{
    public class EstanteRepositorio : IEstante
    {
        private readonly string cadenaConexion;
        private readonly IConfiguration _config;

        public EstanteRepositorio(IConfiguration config)
        {
            _config = config;
            cadenaConexion = _config["ConnectionStrings:DB"];
        }

        public List<Estante> Listado()
        {
            var listado = new List<Estante>();
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("ListarEstantes", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listado.Add(new Estante()
                            {
                                id_estante = reader.GetInt32(0),
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
