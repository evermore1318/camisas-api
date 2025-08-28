using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;
using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data
{
    public class PedidoRepositorio : IPedido
    {
        private readonly string cadenaConexion;
        private readonly IConfiguration _config;
        public PedidoRepositorio(IConfiguration config)
        {
            _config = config;
            cadenaConexion = config["ConnectionStrings:DB"];
        }

        public Pedido Actualizar(Pedido pedido)
        {
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var command = new SqlCommand("ActualizarPedido", conexion))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("id", pedido.IdPedido);
                    command.Parameters.AddWithValue("@descripcion", pedido.Descripcion);
                    command.Parameters.AddWithValue("@proveedor", pedido.IdProveedor);
                    command.Parameters.AddWithValue("@fecha", pedido.Fecha);
                    command.Parameters.AddWithValue("@monto", pedido.Monto);
                    command.Parameters.AddWithValue("estado", pedido.Estado);
                    command.ExecuteNonQuery();
                }
            }
            return pedido;
        }

        public bool Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public List<Pedido> Listado()
        {
            var listado = new List<Pedido>();
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var command = new SqlCommand("ListarPedidos", conexion))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                listado.Add(new Pedido()
                                {
                                    IdPedido = reader.GetInt32(reader.GetOrdinal("id_pedido")),
                                    Descripcion = reader.GetString(reader.GetOrdinal("descripcion")),
                                    IdProveedor = reader.GetInt32(reader.GetOrdinal("id_proveedor")),
                                    Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                                    Monto = reader.GetDecimal(reader.GetOrdinal("monto")),
                                    Estado = reader.GetString(reader.GetOrdinal("estado")),
                                    Proveedor = new Proveedor()
                                    {
                                        IdProveedor = reader.GetInt32(reader.GetOrdinal("id_proveedor")),
                                        Nombre = reader.GetString(reader.GetOrdinal("nombre_proveedor"))
                                    }
                                });
                            }
                        }
                    }
                }
            }
            return listado;
        }

        public Pedido ObtenerPorID(int id)
        {
            Pedido pedido = null;
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("ObtenerPedidoPorID", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@ID", id);
                    using (var reader = comando.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            reader.Read();
                            pedido = new Pedido()
                            {
                                IdPedido = reader.GetInt32(reader.GetOrdinal("id_pedido")),
                                Descripcion = reader.GetString(reader.GetOrdinal("descripcion")),
                                IdProveedor = reader.GetInt32(reader.GetOrdinal("id_proveedor")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                                Monto = reader.GetDecimal(reader.GetOrdinal("monto")),
                                Estado = reader.GetString(reader.GetOrdinal("estado")),
                                CantidadPagos = reader.GetInt32(reader.GetOrdinal("CantidadPagos")),
                                MontoTotalPagos = reader.GetDecimal(reader.GetOrdinal("MontoTotalPagos")),
                                DeudaPendiente = reader.GetDecimal(reader.GetOrdinal("DeudaPendiente"))
                            };
                        }
                    }
                }
            }
            return pedido;
        }

        public Pedido Registrar(Pedido pedido)
        {
            Pedido nuevoPedido = null;
            int nuevoID = 0;

            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("RegistrarPedido", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@descripcion", pedido.Descripcion);
                    comando.Parameters.AddWithValue("@proveedor", pedido.IdProveedor);
                    comando.Parameters.AddWithValue("@fecha", pedido.Fecha);
                    comando.Parameters.AddWithValue("@monto", pedido.Monto);
                    nuevoID = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            nuevoPedido = ObtenerPorID(nuevoID);
            return nuevoPedido;
        }

    }
}
