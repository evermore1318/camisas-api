namespace DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Models
{
    public class DetalleVenta
    {
        public int Id_Venta { get; set; }     
        public int Id_Camisa { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }   
        public string Estado { get; set; } = "Activo";

        
        public Camisa? Camisa { get; set; }
    }
}