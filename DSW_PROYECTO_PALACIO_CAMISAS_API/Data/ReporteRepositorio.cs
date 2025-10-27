using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models.DTOs;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data
{
    public class ReporteRepositorio : IReportes
    {
        // Datos en memoria TEMPORALES para Reportes
        private readonly Random _random = new Random();

        public async Task<IEnumerable<ReporteDiarioDto>> ObtenerReporteDiario(DateTime fecha)
        {
            // Simular datos de reporte diario
            var reportes = new List<ReporteDiarioDto>
            {
                new ReporteDiarioDto { Numero = 1, Boleta = "B-001", Marca = "Nike", Cantidad = 5, Precio = 149.95m },
                new ReporteDiarioDto { Numero = 2, Boleta = "B-002", Marca = "Adidas", Cantidad = 3, Precio = 74.97m },
                new ReporteDiarioDto { Numero = 3, Boleta = "B-003", Marca = "Puma", Cantidad = 2, Precio = 69.98m },
                new ReporteDiarioDto { Numero = 4, Boleta = "B-004", Marca = "Nike", Cantidad = 4, Precio = 119.96m },
                new ReporteDiarioDto { Numero = 5, Boleta = "B-005", Marca = "Adidas", Cantidad = 1, Precio = 24.99m }
            };

            // Simular procesamiento asíncrono
            await Task.Delay(100);

            return reportes;
        }
    }
}