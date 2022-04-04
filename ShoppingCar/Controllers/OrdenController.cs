using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCar.AccesoDatos.Datos.Repositorio.IRepositorio;
using ShoppingCar.Modelos;
using ShoppingCar.Modelos.ViewModels;
using ShoppingCar.Utilidades;

namespace ShoppingCar.Controllers
{
    [Authorize(Roles =WC.AdminRole)]
    public class OrdenController : Controller
    {
        private readonly IOrdenRepositorio _ordenRepositorio;
        private readonly IOrdenDetalleRepositorio _ordenDetalleRepositorio;

        [BindProperty]
        public OrdenVM OrdenVM { get; set; }

        public OrdenController(IOrdenRepositorio ordenRepositorio, IOrdenDetalleRepositorio ordenDetalleRepositorio)
        {
            _ordenRepositorio = ordenRepositorio;
            _ordenDetalleRepositorio = ordenDetalleRepositorio;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detalle(int id)
        {
            OrdenVM = new OrdenVM()
            {
                Orden = _ordenRepositorio.ObtenerPrimero(o => o.Id == id),
                OrdenDetalle = _ordenDetalleRepositorio.ObtenerTodos(d => d.OrdenId == id, incluirPropiedades: "Producto"),
            };
            return View(OrdenVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Detalle()
        {
            List<CarroCompra> carroCompraLista = new List<CarroCompra>();

            OrdenVM.OrdenDetalle = _ordenDetalleRepositorio.ObtenerTodos(d => d.OrdenId == OrdenVM.Orden.Id);
            foreach (var detalle in OrdenVM.OrdenDetalle)
            {
                CarroCompra carroCompra = new CarroCompra()
                {
                    ProductoId = detalle.ProductoId
                };
                carroCompraLista.Add(carroCompra);
            }
            HttpContext.Session.Clear();
            HttpContext.Session.Set(WC.SessionCarroCompras, carroCompraLista);
            HttpContext.Session.Set(WC.SessionOrdenId, OrdenVM.Orden.Id);
            return RedirectToAction("Index", "Carro");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar()
        {
            Orden orden = _ordenRepositorio.ObtenerPrimero(o => o.Id == OrdenVM.Orden.Id);
            IEnumerable<OrdenDetalle> ordenDetalle = _ordenDetalleRepositorio
                                                    .ObtenerTodos(d => d.OrdenId == OrdenVM.Orden.Id);

            _ordenDetalleRepositorio.RemoverRango(ordenDetalle);
            _ordenRepositorio.Remover(orden);

            TempData[WC.Exitosa] = "Registro eliminado de forma exitosa";
            _ordenRepositorio.Grabar();
            return RedirectToAction("Index");

        }
        #region APIs
        [HttpGet]
        public IActionResult ObtenerListaOrdenes()
        {
            return Json(new { data = _ordenRepositorio.ObtenerTodos() });
        }
        #endregion
    }
}
