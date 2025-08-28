using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato
{
    public interface IUsuario
    {
        List<Usuario> ListadoUsuarios();                 
        Usuario ObtenerPorID(int id);            
        Usuario Registrar(Usuario usuario);      
        Usuario ActualizarEstado(int id, string nuevoEstado);
        List<Rol> ListadoRoles();
        bool Eliminar(int id);
    }
}
