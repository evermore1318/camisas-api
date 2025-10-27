using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data
{
    public class PedidoRepositorio : IPedido
    {
        // Datos en memoria TEMPORALES para Pedidos
        private List<Pedido> _pedidos = new List<Pedido>
        {
            new Pedido {
                IdPedido = 1,
                Descripcion = "Pedido de camisas deportivas Nike",
                IdProveedor = 1,
                Fecha = DateTime.Now.AddDays(-10),
                Monto = 1500.00m,
                Estado = "Completado",
                Proveedor = new Proveedor { IdProveedor = 1, Nombre = "Proveedor Nike" },
                CantidadPagos = 2,
                MontoTotalPagos = 1500.00m,
                DeudaPendiente = 0.00m
            },
            new Pedido {
                IdPedido = 2,
                Descripcion = "Pedido de camisas Adidas verano",
                IdProveedor = 2,
                Fecha = DateTime.Now.AddDays(-5),
                Monto = 1200.00m,
                Estado = "En proceso",
                Proveedor = new Proveedor { IdProveedor = 2, Nombre = "Proveedor Adidas" },
                CantidadPagos = 1,
                MontoTotalPagos = 600.00m,
                DeudaPendiente = 600.00m
            },
            new Pedido {
                IdPedido = 3,
                Descripcion = "Pedido camisas Puma temporada",
                IdProveedor = 3,
                Fecha = DateTime.Now.AddDays(-2),
                Monto = 800.00m,
                Estado = "Pendiente",
                Proveedor = new Proveedor { IdProveedor = 3, Nombre = "Proveedor Puma" },
                CantidadPagos = 0,
                MontoTotalPagos = 0.00m,
                DeudaPendiente = 800.00m
            }
        };

        private int _nextId = 4;

        public List<Pedido> Listado()
        {
            return _pedidos;
        }

        public Pedido ObtenerPorID(int id)
        {
            return _pedidos.FirstOrDefault(p => p.IdPedido == id);
        }

        public Pedido Registrar(Pedido pedido)
        {
            pedido.IdPedido = _nextId++;
            pedido.Fecha = DateTime.Now;
            pedido.Estado = "Pendiente";
            pedido.CantidadPagos = 0;
            pedido.MontoTotalPagos = 0;
            pedido.DeudaPendiente = pedido.Monto;
            _pedidos.Add(pedido);
            return pedido;
        }

        public Pedido Actualizar(Pedido pedido)
        {
            var existente = _pedidos.FirstOrDefault(p => p.IdPedido == pedido.IdPedido);
            if (existente != null)
            {
                existente.Descripcion = pedido.Descripcion;
                existente.IdProveedor = pedido.IdProveedor;
                existente.Fecha = pedido.Fecha;
                existente.Monto = pedido.Monto;
                existente.Estado = pedido.Estado;
            }
            return existente;
        }

        public bool Eliminar(int id)
        {
            var pedido = _pedidos.FirstOrDefault(p => p.IdPedido == id);
            if (pedido != null)
            {
                _pedidos.Remove(pedido);
                return true;
            }
            return false;
        }
    }
}