using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data
{
    public class FinanzasRepositorio : IFinanzas
    {
        private readonly string cadenaConexion;
        private readonly IConfiguration _config;
        public FinanzasRepositorio(IConfiguration config)
        {
            _config = config;
            cadenaConexion = config["ConnectionStrings:DB"];
        }

        public decimal ObtenerIngresosMensuales(int anio, int mes)
        {
            decimal ingresos = 0;

            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("sp_IngresosMensuales", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@Anio", anio);
                    comando.Parameters.AddWithValue("@Mes", mes);

                    ingresos = Convert.ToDecimal(comando.ExecuteScalar());
                }
            }

            return ingresos;
        }

        public decimal ObtenerIngresosAnuales(int anio)
        {
            decimal ingresos = 0;

            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("sp_IngresosAnuales", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@Anio", anio);

                    ingresos = Convert.ToDecimal(comando.ExecuteScalar());
                }
            }

            return ingresos;
        }

        // -------------------
        // EGRESOS
        // -------------------
        public decimal ObtenerEgresosMensuales(int anio, int mes)
        {
            decimal egresos = 0;

            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("sp_EgresosMensuales", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@Anio", anio);
                    comando.Parameters.AddWithValue("@Mes", mes);

                    egresos = Convert.ToDecimal(comando.ExecuteScalar());
                }
            }

            return egresos;
        }

        public decimal ObtenerEgresosAnuales(int anio)
        {
            decimal egresos = 0;

            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("sp_EgresosAnuales", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@Anio", anio);

                    egresos = Convert.ToDecimal(comando.ExecuteScalar());
                }
            }

            return egresos;
        }

        public FinanzasDto ObtenerResumen(int anio, int mes)
        {
            return new FinanzasDto
            {
                IngresoMensual = ObtenerIngresosMensuales(anio, mes),
                IngresoAnual = ObtenerIngresosAnuales(anio),
                EgresoMensual = ObtenerEgresosMensuales(anio, mes),
                EgresoAnual = ObtenerEgresosAnuales(anio)
            };
        }
    }
}
