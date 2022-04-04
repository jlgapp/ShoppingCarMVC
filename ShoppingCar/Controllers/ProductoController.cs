using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingCar.AccesoDatos.Datos;
using ShoppingCar.AccesoDatos.Datos.Repositorio.IRepositorio;
using ShoppingCar.Modelos;
using ShoppingCar.Modelos.ViewModels;
using ShoppingCar.Utilidades;

namespace ShoppingCar.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductoController : Controller
    {
        private readonly IProductoRepositorio _context;
        private readonly IWebHostEnvironment _webHostEnvironment; // para poder recibir images

        public ProductoController(IProductoRepositorio context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var include = "Categoria,TipoAplicacion";
            IEnumerable<Producto> lista = _context.ObtenerTodos(incluirPropiedades: include);
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
                /*CategoriaLista = _context.Categoria.Select(c => new SelectListItem
                {
                    Text = c.NombreCategoria,
                    Value = c.Id.ToString()
                }),
                TipoAplicacionLista = _context.TipoAplicacion.Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                })*/
                CategoriaLista = _context.ObtenerTodosDropDownList(WC.CategoriaNombre),
                TipoAplicacionLista = _context.ObtenerTodosDropDownList(WC.TipoAplicacionNombre)
            };



            if (id == null)
                return View(productoVM);
            else
            {
                productoVM.Producto = _context.Obtener(id.GetValueOrDefault());

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
                    using (var stream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(stream);
                    }
                    productoVM.Producto.ImagenUrl = fileName + extension;
                    _context.Agregar(productoVM.Producto);
                }
                else
                {
                    //actualizar con as no tracking no guarda en memoria y no hcae tracking o
                    //contrarestra con el otor objeto que voy a actualizar
                    var objProducto = _context.ObtenerPrimero(p => p.Id == productoVM.Producto.Id, isTracking: false);
                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagenRuta;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        // borrar imagen anterior
                        var anterior = Path.Combine(upload, objProducto.ImagenUrl);

                        if (System.IO.File.Exists(anterior))
                            System.IO.File.Delete(anterior);

                        using (var stream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(stream);
                        }
                        productoVM.Producto.ImagenUrl = fileName + extension;

                    } // no actualiza imagen
                    else
                    {
                        productoVM.Producto.ImagenUrl = objProducto.ImagenUrl;
                    }
                    _context.UpdateEntity(productoVM.Producto);
                }
                _context.Grabar();

                TempData[WC.Exitosa] = "Registro creado/modificado de forma exitosa";
                return RedirectToAction("Index");
            } /// model isvalid

            // llenar nuevamente las listas si algo falla
            /*productoVM.CategoriaLista = _context.Categoria.Select(c => new SelectListItem
            {
                Text = c.NombreCategoria,
                Value = c.Id.ToString()
            });
            productoVM.TipoAplicacionLista = _context.TipoAplicacion.Select(c => new SelectListItem
            {
                Text = c.Nombre,
                Value = c.Id.ToString()
            });*/
            productoVM.CategoriaLista = _context.ObtenerTodosDropDownList(WC.CategoriaNombre);
            productoVM.TipoAplicacionLista = _context.ObtenerTodosDropDownList(WC.TipoAplicacionNombre);

            return View(productoVM);
        }

        //get
        public IActionResult Eliminar(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = _context.ObtenerPrimero(p=> p.Id == Id, incluirPropiedades: "Categoria,TipoAplicacion");

            if (obj == null) return NotFound();

            return View(obj);
        }

        //Post Eliminar 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(Producto producto)
        {
            if (producto == null)
            {
                return NotFound();
            }
            string upload = _webHostEnvironment.WebRootPath + WC.ImagenRuta;

            var anterior = Path.Combine(upload, producto.ImagenUrl);

            if (System.IO.File.Exists(anterior))
                System.IO.File.Delete(anterior);

            _context.Remover(producto);
            _context.Grabar();

            TempData[WC.Exitosa] = "Registro eliminado de forma exitosa";
            return RedirectToAction("Index");

        }


    }
}
