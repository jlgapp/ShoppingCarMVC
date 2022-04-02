using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCar.AccesoDatos.Datos;
using ShoppingCar.AccesoDatos.Datos.Repositorio.IRepositorio;
using ShoppingCar.Modelos;
using ShoppingCar.Modelos.ViewModels;
using ShoppingCar.Utilidades;
using System.Diagnostics;

namespace ShoppingCar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductoRepositorio _productoRepositorio;
        private readonly ICategoriaRepositorio _categoriaRepositorio;

        public HomeController(ILogger<HomeController> logger,
            IProductoRepositorio productoRepositorio,
            ICategoriaRepositorio categoriaRepositorio
            )
        {
            _logger = logger;
            _categoriaRepositorio= categoriaRepositorio ;
            _productoRepositorio= productoRepositorio ;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Productos = _productoRepositorio.ObtenerTodos(incluirPropiedades:"Categoria,TipoAplicacion"),
                Categorias = _categoriaRepositorio.ObtenerTodos()
            };

            return View(homeVM);
        }

        public IActionResult Detalle(int Id)
        {

            List<CarroCompra> carroComprasLista = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null
                && HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }

            DetalleVM detalleVM = new DetalleVM()
            {
                Producto = _productoRepositorio.ObtenerPrimero(t => t.Id == Id,incluirPropiedades:"Categoria,TipoAplicacion"),
                /*Producto = _context.Producto
                                    .Include(c => c.Categoria)
                                    .Include(t => t.TipoAplicacion)
                                    .Where(t => t.Id == Id)
                                    .FirstOrDefault(),*/
                ExisteEnCarro = false
            };
            foreach (var item in carroComprasLista)
            {
                if(item.ProductoId == Id)
                {
                    detalleVM.ExisteEnCarro = true;
                }

            }

            return View(detalleVM);
        }
        [HttpPost, ActionName("Detalle")]
        public IActionResult DetallePost(int Id)
        {
            List<CarroCompra> carroComprasLista = new List<CarroCompra>();
            if(HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null
                && HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            carroComprasLista.Add(new CarroCompra { ProductoId = Id });
            HttpContext.Session.Set(WC.SessionCarroCompras, carroComprasLista);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoverDeCarro(int Id)
        {
            List<CarroCompra> carroComprasLista = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null
                && HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            
            var productoRemover = carroComprasLista.SingleOrDefault(x => x.ProductoId == Id);
            if (productoRemover != null)
            {
                carroComprasLista.Remove(productoRemover);
            }

            HttpContext.Session.Set(WC.SessionCarroCompras, carroComprasLista);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}