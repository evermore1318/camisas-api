using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato
{
    public interface ICamisa
    {
        List<Camisa> Listado();
        Camisa ObtenerPorID(int id);
        Camisa Registrar(Camisa camisa);
        Camisa Actualizar(Camisa camisa);
        bool Eliminar(int id);
    }
}
