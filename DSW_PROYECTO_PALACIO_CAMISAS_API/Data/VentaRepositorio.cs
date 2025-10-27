using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data
{
    public class VentaRepositorio : IVenta
    {
        // Datos en memoria TEMPORALES para Ventas
        private List<Venta> _ventas = new List<Venta>
        {
            new Venta {
                id_venta = 1,
                nombre_cliente = "Juan Pérez",
                dni_cliente = "12345678",
                tipo_pago = "Tarjeta",
                fecha = DateTime.Now.AddDays(-3),
                precio_total = 149.95m,
                estado = "Completado",
                detalles = new List<DetalleVenta>
                {
                    new DetalleVenta { id_venta = 1, id_camisa = 1, cantidad = 2, precio = 29.99m, estado = "Activo",
                                     camisa_descripcion = "Camisa Deportiva Nike", camisa_color = "Azul",
                                     camisa_talla = "M", camisa_manga = "Corta", marca_nombre = "Nike" },
                    new DetalleVenta { id_venta = 1, id_camisa = 2, cantidad = 3, precio = 89.97m, estado = "Activo",
                                     camisa_descripcion = "Camisa Casual Adidas", camisa_color = "Blanca",
                                     camisa_talla = "L", camisa_manga = "Larga", marca_nombre = "Adidas" }
                }
            },
            new Venta {
                id_venta = 2,
                nombre_cliente = "María García",
                dni_cliente = "87654321",
                tipo_pago = "Efectivo",
                fecha = DateTime.Now.AddDays(-1),
                precio_total = 74.97m,
                estado = "Completado",
                detalles = new List<DetalleVenta>
                {
                    new DetalleVenta { id_venta = 2, id_camisa = 3, cantidad = 3, precio = 74.97m, estado = "Activo",
                                     camisa_descripcion = "Camisa Elegante Puma", camisa_color = "Negra",
                                     camisa_talla = "S", camisa_manga = "Larga", marca_nombre = "Puma" }
                }
            },
            new Venta {
                id_venta = 3,
                nombre_cliente = "Carlos López",
                dni_cliente = "11223344",
                tipo_pago = "Transferencia",
                fecha = DateTime.Now,
                precio_total = 119.96m,
                estado = "Pendiente",
                detalles = new List<DetalleVenta>
                {
                    new DetalleVenta { id_venta = 3, id_camisa = 1, cantidad = 4, precio = 119.96m, estado = "Activo",
                                     camisa_descripcion = "Camisa Deportiva Nike", camisa_color = "Azul",
                                     camisa_talla = "M", camisa_manga = "Corta", marca_nombre = "Nike" }
                }
            }
        };

        private int _nextId = 4;

        public List<Venta> Listado()
        {
            return _ventas;
        }

        public Venta ObtenerPorID(int id)
        {
            return _ventas.FirstOrDefault(v => v.id_venta == id);
        }

        public List<DetalleVenta> ObtenerDetallesPorVentaID(int idVenta)
        {
            var venta = _ventas.FirstOrDefault(v => v.id_venta == idVenta);
            return venta?.detalles ?? new List<DetalleVenta>();
        }

        public Venta Registrar(Venta venta, List<DetalleVenta> detalles)
        {
            venta.id_venta = _nextId++;
            venta.fecha = DateTime.Now;
            venta.estado = "Pendiente";
            venta.detalles = detalles;

            // Asignar el id_venta a cada detalle
            foreach (var detalle in detalles)
            {
                detalle.id_venta = venta.id_venta;
                detalle.estado = "Activo";
            }

            _ventas.Add(venta);
            return venta;
        }

        public bool ActualizarEstado(int id, string estado)
        {
            var venta = _ventas.FirstOrDefault(v => v.id_venta == id);
            if (venta != null)
            {
                venta.estado = estado;

                // Actualizar estado de los detalles también
                foreach (var detalle in venta.detalles)
                {
                    detalle.estado = estado;
                }
                return true;
            }
            return false;
        }
    }
}