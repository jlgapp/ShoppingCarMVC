using ShoppingCar.AccesoDatos.Datos.Repositorio.IRepositorio;
using ShoppingCar.Modelos;

namespace ShoppingCar.AccesoDatos.Datos.Repositorio
{
    public class VentaDetalleRepositorio : Repositorio<VentaDetalle>, IVentaDetalleRepositorio
    {
        private readonly ApplicationDbContext _context;

        public VentaDetalleRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Actualizar(VentaDetalle entidad)
        {
            _context.Update(entidad);
        }       

    }
}
