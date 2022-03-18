using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingCar.Datos;
using ShoppingCar.Models;
using ShoppingCar.Models.ViewModels;

namespace ShoppingCar.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Producto> lista = _context.Producto
                                                    .Include(cat => cat.Categoria)
                                                    .Include(ta => ta.TipoAplicacion);
            return View(lista);
        }

        // Get
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> categoriaDropDown = _context.Categoria.Select(c => new SelectListItem
            //{
            //    Text= c.NombreCategoria,
            //    Value = c.Id.ToString()
            //});
            //ViewBag.categoriaDropDown = categoriaDropDown;  

            

            //Producto producto = new Producto();

            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                CategoriaLista = _context.Categoria.Select(c => new SelectListItem
                {
                    Text = c.NombreCategoria,
                    Value = c.Id.ToString()
                }),
                TipoAplicacionLista = _context.TipoAplicacion.Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                })
            };



            if(id == null)
                return View(productoVM);
            else
            {
                productoVM.Producto = _context.Producto.Find(id);
                
                if (productoVM.Producto == null) return NotFound();

                return View(productoVM);
            }

        }
    }
}
