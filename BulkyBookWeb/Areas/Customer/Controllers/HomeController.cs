
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger, IUnitOfWork UnitOfWork)
        {
            _logger = logger;
            _UnitOfWork = UnitOfWork;
        
           
            //{
            //    Count = 1,
            //    CategoryList = _UnitOfWork.Category.GetAll().Select(i => new SelectListItem
            //    {
            //        Text = i.Name,
            //        Value = i.Id.ToString()
            //    }),
            //    Product = new ()
            //};
        }
        

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _UnitOfWork.Product.GetAll(IncludeProperties:"Category,CoverType");
            return View(productList);
        }

        public IActionResult Details(int id)
        {

            ShoppingCart cartObj = new()
            {
                Count = 1,
                Product = _UnitOfWork.Product.GetFirstOrDefault(u => u.Id == id, IncludeProperties: "Category, CoverType"),

            };

            return View(cartObj);
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