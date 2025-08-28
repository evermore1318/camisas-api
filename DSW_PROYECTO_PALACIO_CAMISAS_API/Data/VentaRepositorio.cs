using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;
using Microsoft.Data.SqlClient;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data
{
    public class VentaRepositorio : IVenta
    {
        private readonly string cadenaConexion;
        private readonly IConfiguration _config;

        public VentaRepositorio(IConfiguration config)
        {
            _config = config;
            cadenaConexion = _config["ConnectionStrings:DB"];
        }

        public List<Venta> Listado()
        {
            var listado = new List<Venta>();
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("ListarVentasConDetalles", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = comando.ExecuteReader())
                    {
                        var ventasDict = new Dictionary<int, Venta>();

                        while (reader.Read())
                        {
                            int idVenta = reader.GetInt32(0);

                            if (!ventasDict.ContainsKey(idVenta))
                            {
                                ventasDict[idVenta] = new Venta()
                                {
                                    id_venta = idVenta,
                                    nombre_cliente = reader.GetString(1),
                                    dni_cliente = reader.GetString(2),
                                    tipo_pago = reader.GetString(3),
                                    fecha = reader.GetDateTime(4),
                                    precio_total = reader.GetDecimal(5),
                                    estado = reader.GetString(6),
                                    detalles = new List<DetalleVenta>()
                                };
                            }

                            if (!reader.IsDBNull(7))
                            {
                                var detalle = new DetalleVenta()
                                {
                                    id_venta = idVenta,
                                    id_camisa = reader.GetInt32(7),
                                    cantidad = reader.GetInt32(8),
                                    precio = reader.GetDecimal(9),
                                    estado = reader.GetString(10),
                                    camisa_descripcion = reader.GetString(11),
                                    camisa_color = reader.GetString(12),
                                    camisa_talla = reader.GetString(13),
                                    camisa_manga = reader.GetString(14),
                                    marca_nombre = reader.GetString(15)
                                };

                                ventasDict[idVenta].detalles.Add(detalle);
                            }
                        }

                        listado = ventasDict.Values.ToList();
                    }
                }
            }
            return listado;
        }

        public Venta ObtenerPorID(int id)
        {
            Venta venta = null;
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("ObtenerVentaPorIDConDetalles", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@id_venta", id);
                    using (var reader = comando.ExecuteReader())
                    {
                        var detalles = new List<DetalleVenta>();

                        while (reader.Read())
                        {
                            if (venta == null)
                            {
                                venta = new Venta()
                                {
                                    id_venta = reader.GetInt32(0),
                                    nombre_cliente = reader.GetString(1),
                                    dni_cliente = reader.GetString(2),
                                    tipo_pago = reader.GetString(3),
                                    fecha = reader.GetDateTime(4),
                                    precio_total = reader.GetDecimal(5),
                                    estado = reader.GetString(6),
                                    detalles = new List<DetalleVenta>()
                                };
                            }

                            if (!reader.IsDBNull(7))
                            {
                                var detalle = new DetalleVenta()
                                {
                                    id_venta = id,
                                    id_camisa = reader.GetInt32(7),
                                    cantidad = reader.GetInt32(8),
                                    precio = reader.GetDecimal(9),
                                    estado = reader.GetString(10),
                                    camisa_descripcion = reader.GetString(11),
                                    camisa_color = reader.GetString(12),
                                    camisa_talla = reader.GetString(13),
                                    camisa_manga = reader.GetString(14),
                                    marca_nombre = reader.GetString(15)
                                };

                                venta.detalles.Add(detalle);
                            }
                        }
                    }
                }
            }
            return venta;
        }

        public List<DetalleVenta> ObtenerDetallesPorVentaID(int idVenta)
        {
            var detalles = new List<DetalleVenta>();
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("ObtenerDetallesPorVenta", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@id_venta", idVenta);
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            detalles.Add(new DetalleVenta()
                            {
                                id_venta = idVenta,
                                id_camisa = reader.GetInt32(0),
                                cantidad = reader.GetInt32(1),
                                precio = reader.GetDecimal(2),
                                estado = reader.GetString(3),
                                camisa_descripcion = reader.GetString(4),
                                camisa_color = reader.GetString(5),
                                camisa_talla = reader.GetString(6),
                                camisa_manga = reader.GetString(7),
                                marca_nombre = reader.GetString(8)
                            });
                        }
                    }
                }
            }
            return detalles;
        }

        public Venta Registrar(Venta venta, List<DetalleVenta> detalles)
        {
            Venta nuevaVenta = null;
            int nuevoId = 0;

            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        using (var comando = new SqlCommand("RegistrarVenta", conexion, transaccion))
                        {
                            comando.CommandType = System.Data.CommandType.StoredProcedure;
                            comando.Parameters.AddWithValue("@nombre_cliente", venta.nombre_cliente);
                            comando.Parameters.AddWithValue("@dni_cliente", venta.dni_cliente);
                            comando.Parameters.AddWithValue("@tipo_pago", venta.tipo_pago);
                            comando.Parameters.AddWithValue("@fecha", venta.fecha);
                            comando.Parameters.AddWithValue("@precio_total", venta.precio_total);
                            comando.Parameters.AddWithValue("@estado", venta.estado);
                            nuevoId = Convert.ToInt32(comando.ExecuteScalar());
                        }

                        foreach (var detalle in detalles)
                        {
                            using (var comando = new SqlCommand("RegistrarDetalleVenta", conexion, transaccion))
                            {
                                comando.CommandType = System.Data.CommandType.StoredProcedure;
                                comando.Parameters.AddWithValue("@id_venta", nuevoId);
                                comando.Parameters.AddWithValue("@id_camisa", detalle.id_camisa);
                                comando.Parameters.AddWithValue("@cantidad", detalle.cantidad);
                                comando.Parameters.AddWithValue("@precio", detalle.precio);
                                comando.Parameters.AddWithValue("@estado", detalle.estado);
                                comando.ExecuteNonQuery();
                            }
                        }

                        transaccion.Commit();
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
            nuevaVenta = ObtenerPorID(nuevoId);
            return nuevaVenta;
        }

        public bool ActualizarEstado(int id, string estado)
        {
            var exito = false;
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var transaccion = conexion.BeginTransaction())
                {
                    try
                    {

                        using (var comando = new SqlCommand("ActualizarEstadoVenta", conexion, transaccion))
                        {
                            comando.CommandType = System.Data.CommandType.StoredProcedure;
                            comando.Parameters.AddWithValue("@id_venta", id);
                            comando.Parameters.AddWithValue("@estado", estado);
                            exito = comando.ExecuteNonQuery() > 0;
                        }

                        if (exito)
                        {
                            using (var comando = new SqlCommand("ActualizarEstadoDetalleVenta", conexion, transaccion))
                            {
                                comando.CommandType = System.Data.CommandType.StoredProcedure;
                                comando.Parameters.AddWithValue("@id_venta", id);
                                comando.Parameters.AddWithValue("@estado", estado);
                                comando.ExecuteNonQuery();
                            }
                        }

                        transaccion.Commit();
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
            return exito;
        }
    }
}
