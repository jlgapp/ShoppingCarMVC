using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCar.AccesoDatos.Datos;
using ShoppingCar.AccesoDatos.Datos.Repositorio.IRepositorio;
using ShoppingCar.Modelos;
using ShoppingCar.Utilidades;

namespace ShoppingCar.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoriaController : Controller
    {
        private readonly ICategoriaRepositorio _context;

        public CategoriaController(ICategoriaRepositorio context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Categoria> lista = _context.ObtenerTodos();

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
                _context.Agregar(categoria);
                _context.Grabar();
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
            var obj = _context.Obtener(IdCat.GetValueOrDefault());
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
                //_context.Categoria.Update(categoria);
                _context.UpdateEntity(categoria);
                _context.Grabar();
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
            var obj = _context.Obtener(IdCat.GetValueOrDefault());
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
            _context.Remover(categoria);
            _context.Grabar();
            return RedirectToAction("Index");

        }

    }
}
