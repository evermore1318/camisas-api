namespace DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; } 
        public int IdRol { get; set; }
        public string RolDescripcion { get; set; }
        public string Estado { get; set; }
    }
}
