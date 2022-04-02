using ShoppingCar.AccesoDatos.Datos.Repositorio.IRepositorio;
using ShoppingCar.Modelos;

namespace ShoppingCar.AccesoDatos.Datos.Repositorio
{
    public class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
    {
        private readonly ApplicationDbContext _context;

        public CategoriaRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Actualizar(Categoria categoria)
        {
            var cateAnterior = _context.Categoria.FirstOrDefault( c=> c.Id == categoria.Id );
            if ( cateAnterior != null )
            {
                cateAnterior.NombreCategoria = categoria.NombreCategoria;
                cateAnterior.MostrarOrden = categoria.MostrarOrden;
            }
        }
    }
}
