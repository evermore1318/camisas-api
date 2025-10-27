using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models.DTOs;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data
{
    public class FinanzasRepositorio : IFinanzas
    {
        // Datos en memoria TEMPORALES para Finanzas
        private readonly Random _random = new Random();

        public decimal ObtenerIngresosMensuales(int anio, int mes)
        {
            // Simular ingresos mensuales entre $5000 y $15000
            return _random.Next(5000, 15001);
        }

        public decimal ObtenerIngresosAnuales(int anio)
        {
            // Simular ingresos anuales (suma de meses)
            decimal total = 0;
            for (int mes = 1; mes <= 12; mes++)
            {
                total += ObtenerIngresosMensuales(anio, mes);
            }
            return total;
        }

        public decimal ObtenerEgresosMensuales(int anio, int mes)
        {
            // Simular egresos mensuales entre $3000 y $8000
            return _random.Next(3000, 8001);
        }

        public decimal ObtenerEgresosAnuales(int anio)
        {
            // Simular egresos anuales (suma de meses)
            decimal total = 0;
            for (int mes = 1; mes <= 12; mes++)
            {
                total += ObtenerEgresosMensuales(anio, mes);
            }
            return total;
        }

        public FinanzasDto ObtenerResumen(int anio, int mes)
        {
            return new FinanzasDto
            {
                IngresoMensual = ObtenerIngresosMensuales(anio, mes),
                IngresoAnual = ObtenerIngresosAnuales(anio),
                EgresoMensual = ObtenerEgresosMensuales(anio, mes),
                EgresoAnual = ObtenerEgresosAnuales(anio)
            };
        }
    }
}