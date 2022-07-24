
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Route("Product")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        
        //[BindProperty]
        //public ProductViewModel ProductVM { get; set; }
        public CompanyController(IUnitOfWork UnitOfWork)
        {

            _UnitOfWork = UnitOfWork;
           
        }

        //[Route("")]
        //[Route("~")]
        //[Route("Index")]
        public IActionResult Index()
        {
           
            return View();
        }

        [HttpGet]
        public IActionResult Upsert(int? Id)
        {

            Company company = new();

            if (Id == null || Id == 0)
            {
                return View(company);
            }
            else
            {
                company = _UnitOfWork.Company.GetFirstOrDefault(u => u.Id == Id);
                return View(company);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {
   
                if(obj.Id == 0)
                {
                    _UnitOfWork.Company.Add(obj);
                    TempData["success"] = "Product Created Successfully";
                }
                else
                {
                    _UnitOfWork.Company.Update(obj);
                    TempData["success"] = "Product Updated Successfully";
                }
               
                _UnitOfWork.Save();
                
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }


        #region API CALLS
        [HttpGet]
       
        //[Route("GetAll")]
        public IActionResult GetAll()
        {
            var companyList = _UnitOfWork.Company.GetAll();
            return Json(new { data = companyList });
        }

        [HttpDelete]
       
        public IActionResult Delete(int? id)
        {
            var obj = _UnitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _UnitOfWork.Company.Remove(obj);
            _UnitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion

    }

}
