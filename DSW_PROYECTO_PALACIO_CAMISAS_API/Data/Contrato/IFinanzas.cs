using DSW_PROYECTO_PALACIO_CAMISAS_API.Models.DTOs;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato
{
    public interface IFinanzas
    {
        decimal ObtenerIngresosMensuales(int anio, int mes);
        decimal ObtenerIngresosAnuales(int anio);
        decimal ObtenerEgresosMensuales(int anio, int mes);
        decimal ObtenerEgresosAnuales(int anio);

        FinanzasDto ObtenerResumen(int anio, int mes);
    }
}
