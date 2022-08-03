using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    [BindProperties]
    public class CartController : Controller
    {
       
        private readonly IUnitOfWork _UnitOfWork;
             
        public CartController(IUnitOfWork UnitOfWork)
        {

            _UnitOfWork = UnitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM shoppingCartVM =new ShoppingCartVM()
            {
               

                //ListCart = _UnitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, IncludeProperties: "Product")

                //navigation property not populating here too
                ListCart = _UnitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, IncludeProperties:"Category,CoverType")


            };
            return Json(shoppingCartVM);
        }
    }
}
