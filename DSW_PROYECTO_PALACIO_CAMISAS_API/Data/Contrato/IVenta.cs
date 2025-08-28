using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato
{
    public interface IVenta
    {
        List<Venta> Listado();
        Venta ObtenerPorID(int id);
        List<DetalleVenta> ObtenerDetallesPorVentaID(int idVenta);
        Venta Registrar(Venta venta, List<DetalleVenta> detalles);
        bool ActualizarEstado(int id, string estado);
    }
}
