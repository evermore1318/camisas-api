using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato
{
    public interface IPedido
    {
        List<Pedido> Listado();
        Pedido ObtenerPorID(int id);
        Pedido Registrar(Pedido pedido);
        Pedido Actualizar(Pedido pedido);
        bool Eliminar(int id);
    }
}
