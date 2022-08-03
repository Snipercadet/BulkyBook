
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [BindProperties]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly ILogger<HomeController> _logger;
        //public ShoppingCart shoppingCart1 { get; set; }
        //public Category category { get; set; }
       



        public HomeController(ILogger<HomeController> logger, IUnitOfWork UnitOfWork)
        {
            _logger = logger;
            _UnitOfWork = UnitOfWork;

            

        }
        

        public IActionResult Index()
        {

            //the navigation propeerties for category and covertype is not being displayed at this point
            
            var productList = _UnitOfWork.Product.GetAll(IncludeProperties: "Category").ToList();
            return Json(productList);
        }

        public IActionResult Details(int productid)
        {
      
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Count = 1,
                ProductId = productid,
                //the navigation propeerties for category and covertype is not being displayed at this point
                Product = _UnitOfWork.Product.GetFirstOrDefault(u=>u.Id==productid, IncludeProperties: "Category,CoverType")


            };

            return Json(shoppingCart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            ShoppingCart CartFromDB = _UnitOfWork.ShoppingCart.GetFirstOrDefault(u => u.ApplicationUserId == claim.Value && u.ProductId == shoppingCart.ProductId);
            if (CartFromDB == null)
            {
                _UnitOfWork.ShoppingCart.Add(shoppingCart);
            }
            else
            {
                _UnitOfWork.ShoppingCart.IncreamentCount(CartFromDB, shoppingCart.Count);  
            }

            _UnitOfWork.Save();
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