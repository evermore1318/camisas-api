namespace DSW_PROYECTO_PALACIO_CAMISAS_WebApp.ViewModels
{
    public class DetalleVentaTotal
    {
        public int Id_Camisa { get; set; }
        public string Descripcion { get; set; } = "";
        public string Presentacion { get; set; } = ""; // color/talla/manga
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}
