using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data
{
    public class ReporteRepositorio : IReportes
    {
        private readonly string cadenaConexion;
        private readonly IConfiguration _config;

        public ReporteRepositorio(IConfiguration config)
        {
            _config = config;
            cadenaConexion = config["ConnectionStrings:DB"];
        }

        public async Task<IEnumerable<ReporteDiarioDto>> ObtenerReporteDiario(DateTime fecha)
        {
            var lista = new List<ReporteDiarioDto>();

            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_reporte_diario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fecha_reporte", fecha);

                    await con.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lista.Add(new ReporteDiarioDto
                            {
                                Numero = reader.GetInt32(0),
                                Marca = reader.GetString(1),
                                Cantidad = reader.GetInt32(2),
                                Precio = reader.GetDecimal(3)
                            });
                        }
                    }
                }
            }

            return lista;
        }
    }
}
