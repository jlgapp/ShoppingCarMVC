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
        private readonly IWebHostEnvironment _webHostEnvironment; // para poder recibir images

        public ProductoController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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

        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productoVM.Producto.Id == 0)
                {
                    string upload = webRootPath + WC.ImagenRuta;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);
                    using(var stream = new FileStream(Path.Combine(upload,fileName + extension),FileMode.Create))
                    {
                        files[0].CopyTo(stream);
                    }
                    productoVM.Producto.ImagenUrl = fileName + extension;
                    _context.Producto.Add(productoVM.Producto);
                }
                else
                {
                    //actualizar
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            } /// model iscalid
            return View(productoVM);
        }


    }
}
