using ShoppingCar.Modelos;

namespace ShoppingCar.AccesoDatos.Datos.Repositorio.IRepositorio
{
    public interface ICategoriaRepositorio : IRepositorio<Categoria>
    {
        void Actualizar(Categoria categoria);

    }
}
