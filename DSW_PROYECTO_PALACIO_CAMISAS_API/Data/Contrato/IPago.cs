using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato
{
    public interface IPago
    {
        
        Pago ObtenerPorID(int id);

        Pago Registrar(Pago pago);
    }
}
