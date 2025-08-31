namespace DSW_PROYECTO_PALACIO_CAMISAS_WebApp.ViewModels
{
    public class VentaCreate
    {

        public string Nombre_Cliente { get; set; } = "";
        public string Dni_Cliente { get; set; } = "";
        public string Tipo_Pago { get; set; } = "Efectivo";

        public int? CamisaSeleccionadaId { get; set; }
        public int Cantidad { get; set; } = 1;
        public decimal PrecioUnitario { get; set; }

        // Detalle completo
        public List<DetalleVentaTotal> Lineas { get; set; } = new();

        public decimal Total => Lineas.Sum(l => l.Subtotal);
    }
}
