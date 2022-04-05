using ShoppingCar.AccesoDatos.Datos.Repositorio.IRepositorio;
using ShoppingCar.Modelos;

namespace ShoppingCar.AccesoDatos.Datos.Repositorio
{
    public class VentaRepositorio : Repositorio<Venta>, IVentaRepositorio
    {
        private readonly ApplicationDbContext _context;

        public VentaRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Actualizar(Venta entidad)
        {
            _context.Update(entidad);
        }       

    }
}
