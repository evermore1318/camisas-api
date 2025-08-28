using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data
{
    public class PagoRepositorio : IPago
    {
        private readonly string cadenaConexion;
        private readonly IConfiguration _config;
        public PagoRepositorio(IConfiguration config)
        {
            _config = config;
            cadenaConexion = config["ConnectionStrings:DB"];
        }

        public Pago ObtenerPorID(int id)
        {
            Pago pago = null;
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("ObtenerPagoPorID", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@ID", id);
                    using (var reader = comando.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            reader.Read();
                            pago = new Pago()
                            {
                                IdPago = reader.GetInt32(reader.GetOrdinal("id_pago")),
                                IdPedido = reader.GetInt32(reader.GetOrdinal("id_pedido")),
                                Descripcion = reader.GetString(reader.GetOrdinal("descripcion")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                                Monto = reader.GetDecimal(reader.GetOrdinal("monto")),
                                Estado = reader.GetString(reader.GetOrdinal("estado")),
                            };
                        }
                    }
                }
            }
            return pago;
        }

        public Pago Registrar(Pago pago)
        {
            Pago nuevoPago = null;
            int nuevoID = 0;

            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("RegistrarPago", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@idPedido", pago.IdPedido);
                    comando.Parameters.AddWithValue("@descripcion", pago.Descripcion);
                    comando.Parameters.AddWithValue("@fecha", pago.Fecha);
                    comando.Parameters.AddWithValue("@monto", pago.Monto);

                    nuevoID = Convert.ToInt32(comando.ExecuteScalar());
                }
            }

            nuevoPago = ObtenerPorID(nuevoID);

            return nuevoPago;
        }
    }
}
