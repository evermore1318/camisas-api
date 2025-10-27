using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data
{
    public class PagoRepositorio : IPago
    {
        // Datos en memoria TEMPORALES para Pagos
        private List<Pago> _pagos = new List<Pago>
        {
            new Pago { IdPago = 1, IdPedido = 1, Descripcion = "Pago inicial pedido #001", Fecha = DateTime.Now.AddDays(-5), Monto = 150.50m, Estado = "Completado" },
            new Pago { IdPago = 2, IdPedido = 2, Descripcion = "Pago pedido #002", Fecha = DateTime.Now.AddDays(-3), Monto = 89.99m, Estado = "Completado" },
            new Pago { IdPago = 3, IdPedido = 3, Descripcion = "Anticipo pedido #003", Fecha = DateTime.Now.AddDays(-1), Monto = 200.00m, Estado = "Pendiente" }
        };

        private int _nextId = 4;

        public Pago ObtenerPorID(int id)
        {
            return _pagos.FirstOrDefault(p => p.IdPago == id);
        }

        public Pago Registrar(Pago pago)
        {
            pago.IdPago = _nextId++;
            pago.Fecha = DateTime.Now;
            pago.Estado = "Completado";
            _pagos.Add(pago);
            return pago;
        }
    }
}