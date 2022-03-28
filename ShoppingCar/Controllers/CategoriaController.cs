using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCar.Datos;
using ShoppingCar.Models;

namespace ShoppingCar.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoriaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Categoria> lista = _context.Categoria;

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
        public IActionResult Crear(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Categoria.Add(categoria);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoria);


        }

        //Get Editar
        public IActionResult Editar(int? IdCat)
        {
            if (IdCat == null || IdCat == 0)
            {
                return NotFound();
            }
            var obj = _context.Categoria.Find(IdCat);
            if (obj == null) return NotFound();


            return View(obj);
        }

        //Post Editar 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Categoria.Update(categoria);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoria);


        }


        //Get Editar
        public IActionResult Eliminar(int? IdCat)
        {
            if (IdCat == null || IdCat == 0)
            {
                return NotFound();
            }
            var obj = _context.Categoria.Find(IdCat);
            if (obj == null) return NotFound();


            return View(obj);
        }

        //Post Editar 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(Categoria categoria)
        {
            if (categoria ==null)
            {
                return NotFound();
            }
            _context.Categoria.Remove(categoria);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }

    }
}
