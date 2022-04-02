using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCar.AccesoDatos.Datos;
using ShoppingCar.AccesoDatos.Datos.Repositorio.IRepositorio;
using ShoppingCar.Modelos;
using ShoppingCar.Utilidades;

namespace ShoppingCar.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class TipoAplicacionController : Controller
    {
        private readonly IRepositorio<TipoAplicacion> _context;

        public TipoAplicacionController(IRepositorio<TipoAplicacion> context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<TipoAplicacion> lista = _context.ObtenerTodos();

            return View(lista);
        }

        //Get
        public IActionResult Crear()
        {
            return View();
        }

        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(TipoAplicacion tipoAplicacion)
        {
            if (ModelState.IsValid)
            {
                _context.Agregar(tipoAplicacion);
                _context.Grabar();
                return RedirectToAction("Index");
            }
            return View(tipoAplicacion);
        }

        //Get Editar
        public IActionResult Editar(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = _context.Obtener(Id.GetValueOrDefault());
            if (obj == null) return NotFound();


            return View(obj);
        }

        //Post Editar 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(TipoAplicacion tipoAplicacion)
        {
            if (ModelState.IsValid)
            {
                _context.UpdateEntity(tipoAplicacion);
                _context.Grabar();
                return RedirectToAction("Index");
            }
            return View(tipoAplicacion);


        }


        //Get Eliminar
        public IActionResult Eliminar(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = _context.Obtener(Id.GetValueOrDefault());
            if (obj == null) return NotFound();


            return View(obj);
        }

        //Post Eliminar 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(TipoAplicacion tipoAplicacion)
        {
            if (tipoAplicacion == null)
            {
                return NotFound();
            }
            _context.Remover(tipoAplicacion);
            _context.Grabar();
            return RedirectToAction("Index");

        }

    }
}
