using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCar.AccesoDatos.Datos;
using ShoppingCar.AccesoDatos.Datos.Repositorio;
using ShoppingCar.AccesoDatos.Datos.Repositorio.IRepositorio;
using ShoppingCar.Modelos;
using ShoppingCar.Modelos.ViewModels;
using ShoppingCar.Utilidades;
using System.Security.Claims;
using System.Text;

namespace ShoppingCar.Controllers
{
    [Authorize]
    public class CarroController : Controller
    {
        //private readonly ApplicationDbContext _context;

        private readonly IProductoRepositorio _productoRepositorio;
        private readonly IUsuarioAplicacionRepositorio _usuarioRepositorio;
        private readonly IOrdenRepositorio _ordenRepositorio;
        private readonly IOrdenDetalleRepositorio _ordenDetalleRepositorio;

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public ProductoUsuarioVM productoUsuarioVM { get; set; }

        public CarroController(IWebHostEnvironment webHostEnvironment,
                                IEmailSender emailSender,
                                IProductoRepositorio productoRepositorio,
                                IUsuarioAplicacionRepositorio usuarioRepositorio,
                                IOrdenRepositorio ordenRepositorio,
                                IOrdenDetalleRepositorio ordenDetalleRepositorio
                                )
        {
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
            _productoRepositorio = productoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _ordenRepositorio = ordenRepositorio;
            _ordenDetalleRepositorio = ordenDetalleRepositorio;
        }

        public IActionResult Index()
        {

            List<CarroCompra> carroCompraList = new List<CarroCompra>();

            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null
                &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0
                )
            {
                carroCompraList = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            List<int> prodEnCarro = carroCompraList.Select(i => i.ProductoId).ToList();

            IEnumerable<Producto> prodList = _productoRepositorio.ObtenerTodos(p => prodEnCarro.Contains(p.Id));
            List<Producto> prodListFinal = new List<Producto>();

            foreach (var objCarro in carroCompraList)
            {
                Producto prod = prodList.FirstOrDefault(p => p.Id == objCarro.ProductoId);
                prod.TempMetroCuadrado = objCarro.MetroCuadrado;
                prodListFinal.Add(prod);
            }

            return View(prodListFinal);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost(IEnumerable<Producto> ProdLista)
        {
            List<CarroCompra> carroCompraLista = new List<CarroCompra>();
            foreach (Producto prod in ProdLista)
            {
                carroCompraLista.Add(new CarroCompra
                {
                    MetroCuadrado = prod.TempMetroCuadrado,
                    ProductoId = prod.Id,
                });
            }
            HttpContext.Session.Set(WC.SessionCarroCompras, carroCompraLista);

            return RedirectToAction(nameof(Resumen));
        }

        public IActionResult Resumen()
        {

            UsuarioAplicacion usuarioAplicacion;

            if (User.IsInRole(WC.AdminRole))
            {
                if (HttpContext.Session.Get<int>(WC.SessionOrdenId) != 0)
                {
                    Orden orden = _ordenRepositorio.ObtenerPrimero(u =>
                                                    u.Id == HttpContext.Session.Get<int>(WC.SessionOrdenId));

                    usuarioAplicacion = new UsuarioAplicacion()
                    {
                        Email = orden.Email,
                        NombreCompleto = orden.NombreCompleto,
                        PhoneNumber = orden.Telefono
                    };
				}
				else
				{
                    usuarioAplicacion = new UsuarioAplicacion();
                }
			}
			else
			{
                // traer usuario conectado
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                //

                usuarioAplicacion = _usuarioRepositorio.ObtenerPrimero(u => u.Id== claims.Value);
            }




            List<CarroCompra> carroCompraList = new List<CarroCompra>();

            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null
                &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0
                )
            {
                carroCompraList = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            List<int> prodEnCarro = carroCompraList.Select(i => i.ProductoId).ToList();

            //IEnumerable<Producto> prodList = _productoRepositorio.ObtenerTodos(p => prodEnCarro.Contains(p.Id));

            productoUsuarioVM = new ProductoUsuarioVM()
            {
                //UsuarioAplicacion = _context.UsuarioAplicacion.FirstOrDefault(u => u.Id == claims.Value),
                UsuarioAplicacion = usuarioAplicacion,
                //ProductoLista = prodList.ToList()
            };

			foreach (var carro in carroCompraList)
			{
                Producto prodTemp = _productoRepositorio.ObtenerPrimero(p => p.Id == carro.ProductoId);
                prodTemp.TempMetroCuadrado = carro.MetroCuadrado;
                productoUsuarioVM.ProductoLista.Add(prodTemp);
            }

            return View(productoUsuarioVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Resumen")]
        public async Task<IActionResult> ResumenPost(ProductoUsuarioVM productoUsuarioVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            var rutaTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                                + "templates" + Path.DirectorySeparatorChar.ToString()
                                + "PlantillaOrden.html";
            ;
            var subject = "Nueva Orden";
            string HtmlBody = "";

            using (StreamReader sr = System.IO.File.OpenText(rutaTemplate))
            {
                HtmlBody = sr.ReadToEnd();
            }
            StringBuilder productoListaSB = new StringBuilder();
            foreach (var prod in productoUsuarioVM.ProductoLista)
            {
                productoListaSB.Append($" - Nmbre: {prod.NombreProducto} <span style='font-size:14px;'>)(ID: {prod.Id})</span> <br />");

            }
            string messageBody = string.Format(HtmlBody,
                                                productoUsuarioVM.UsuarioAplicacion.NombreCompleto,
                                                productoUsuarioVM.UsuarioAplicacion.Email,
                                                productoUsuarioVM.UsuarioAplicacion.PhoneNumber,
                                                productoListaSB.ToString()
                );

            await _emailSender.SendEmailAsync(WC.EmailAdmin, subject, messageBody);

            // grabar la orden y detalle

            Orden orden = new Orden()
            {
                UsuarioAplicacionId = claims.Value,
                Email = productoUsuarioVM.UsuarioAplicacion.Email,
                NombreCompleto = productoUsuarioVM.UsuarioAplicacion.NombreCompleto,
                Telefono = productoUsuarioVM.UsuarioAplicacion.PhoneNumber,
                FechaOrden = DateTime.Now
            };
            //_ordenRepositorio.Agregar(orden);
            //_ordenRepositorio.Grabar();

            foreach (var prod in productoUsuarioVM.ProductoLista)
            {
                OrdenDetalle ordenDetalle = new OrdenDetalle()
                {
                    //OrdenId = orden.Id,
                    ProductoId = prod.Id,
                    Orden = orden
                };
                _ordenDetalleRepositorio.Agregar(ordenDetalle);

            }
            _ordenDetalleRepositorio.Grabar();

            TempData[WC.Exitosa] = "Registro creado de forma exitosa";

            return RedirectToAction(nameof(Confirmacion));
        }
        public IActionResult Confirmacion()
        {
            HttpContext.Session.Clear();
            return View();
        }

        public IActionResult Remover(int Id)
        {

            List<CarroCompra> carroCompraList = new List<CarroCompra>();

            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null
                &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0
                )
            {
                carroCompraList = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            carroCompraList.Remove(carroCompraList.FirstOrDefault(p => p.ProductoId == Id));

            HttpContext.Session.Set(WC.SessionCarroCompras, carroCompraList);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarCarro(IEnumerable<Producto> ProdLista)
        {
            List<CarroCompra> carroCompraLista = new List<CarroCompra>();
            foreach (Producto prod in ProdLista)
            {
                carroCompraLista.Add(new CarroCompra
                {
                    MetroCuadrado = prod.TempMetroCuadrado,
                    ProductoId = prod.Id,
                });
            }
            HttpContext.Session.Set(WC.SessionCarroCompras, carroCompraLista);
            return RedirectToAction("Index");
        }
    }
}
