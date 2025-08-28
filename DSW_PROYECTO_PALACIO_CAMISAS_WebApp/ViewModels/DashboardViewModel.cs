using DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Models;
namespace DSW_PROYECTO_PALACIO_CAMISAS_WebApp.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalCamisas { get; set; }
        public int TotalMarcas { get; set; }
        public int TotalUsuarios { get; set; }
        public int TotalVentas { get; set; }

        public decimal IngresosTotales { get; set; }
        public decimal IngresosHoy { get; set; }

        public int ProductosBajoStock { get; set; } 

        public List<Venta> UltimasVentas { get; set; } = new List<Venta>();
    }
}
