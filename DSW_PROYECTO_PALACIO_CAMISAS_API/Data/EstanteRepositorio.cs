using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data
{
    public class EstanteRepositorio : IEstante
    {
        // Datos en memoria TEMPORALES para Estantes
        private List<Estante> _estantes = new List<Estante>
        {
            new Estante { id_estante = 1, descripcion = "Estante Principal - Piso 1", estado = "Activo" },
            new Estante { id_estante = 2, descripcion = "Estante Secundario - Piso 1", estado = "Activo" },
            new Estante { id_estante = 3, descripcion = "Estante Deportivas - Piso 2", estado = "Activo" },
            new Estante { id_estante = 4, descripcion = "Estante Formales - Piso 2", estado = "Activo" },
            new Estante { id_estante = 5, descripcion = "Estante Ofertas - Piso 3", estado = "Inactivo" }
        };

        public List<Estante> Listado()
        {
            return _estantes.Where(e => e.estado == "Activo").ToList();
        }
    }
}