using System.Linq.Expressions;

namespace ShoppingCar.AccesoDatos.Datos.Repositorio.IRepositorio
{
    public interface IRepositorio<T> where T : class
    {
        T Obtener(int id);
        IEnumerable<T> ObtenerTodos(
            Expression<Func<T, bool>> filtro = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> oderBy = null,
            string incluirPropiedades = null,
            bool isTracking = true
            );
        T ObtenerPrimero(
            Expression<Func<T, bool>> filtro = null,
            string incluirPropiedades = null,
            bool isTracking = true
            );

        void Agregar(T entidad);
        void Remover(T entidad);
        void RemoverRango (IEnumerable<T> entidad);
        void Grabar();
        public void UpdateEntity(T entity);
    }
}
