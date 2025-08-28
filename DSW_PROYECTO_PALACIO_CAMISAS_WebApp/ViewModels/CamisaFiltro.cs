namespace DSW_PROYECTO_PALACIO_CAMISAS_WebApp.ViewModels
{
    public class CamisaFiltro
    {
        public int? MarcaId { get; set; }
        public string? Tipo { get; set; }   // mapea a Descripcion
        public string? Talla { get; set; }
        public string? Manga { get; set; }
        public string? Color { get; set; }
    }
}
