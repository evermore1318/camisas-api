using DSW_PROYECTO_PALACIO_CAMISAS_API.Models.DTOs;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato
{
    public interface IReportes
    {
        Task<IEnumerable<ReporteDiarioDto>> ObtenerReporteDiario(DateTime fecha);
    }
}
