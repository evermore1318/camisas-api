namespace DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Models
{
    public class DetalleVenta
    {
        public int Id_Venta { get; set; }      // el back lo rellena al crear
        public int Id_Camisa { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }    // unitario o importe: aquí unitario
        public string Estado { get; set; } = "Activo";

        // 🔹 Propiedad de navegación (opcional, no mapeada si tu back no la da)
        public Camisa? Camisa { get; set; }
    }
}
