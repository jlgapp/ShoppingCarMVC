using Microsoft.EntityFrameworkCore;
using ShoppingCar.AccesoDatos.Datos.Repositorio.IRepositorio;
using System.Linq.Expressions;

namespace ShoppingCar.AccesoDatos.Datos.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;

        public Repositorio(ApplicationDbContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>();
        }

        public void Agregar(T entidad)
        {
            dbSet.Add(entidad);
        }

        public void Grabar()
        {
            _context.SaveChanges();
        }

        public T Obtener(int id)
        {
            return dbSet.Find(id);
        }

        public T ObtenerPrimero(Expression<Func<T, bool>> filtro = null, string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;

            if (filtro != null)
                query = query.Where(filtro);

            if (incluirPropiedades != null)
            {
                foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp);
                }
            }
            
            if (!isTracking)
                query = query.AsNoTracking();

            return query.FirstOrDefault();
        }

        public IEnumerable<T> ObtenerTodos(Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;

            if (filtro != null)
                query = query.Where(filtro);

            if (incluirPropiedades != null)
            {
                foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp);
                }
            }

            if (orderBy != null)
                query = orderBy(query);

            if (!isTracking)
                query = query.AsNoTracking();

            return query.ToList();
        }

        public void Remover(T entidad)
        {
            dbSet.Remove(entidad);
        }

        public void UpdateEntity(T entity)
        {
            dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
