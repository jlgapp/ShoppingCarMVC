using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCar.Datos;
using ShoppingCar.Models;

namespace ShoppingCar.Controllers
{
    [Authorize(Roles =WC.AdminRole)]
    public class TipoAplicacionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TipoAplicacionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<TipoAplicacion> lista = _context.TipoAplicacion;

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
                _context.TipoAplicacion.Add(tipoAplicacion);
                _context.SaveChanges();
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
            var obj = _context.TipoAplicacion.Find(Id);
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
                _context.TipoAplicacion.Update(tipoAplicacion);
                _context.SaveChanges();
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
            var obj = _context.TipoAplicacion.Find(Id);
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
            _context.TipoAplicacion.Remove(tipoAplicacion);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }

    }
}
