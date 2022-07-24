
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
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private IWebHostEnvironment _hostEnvironment;
        [BindProperty]
        public ProductViewModel ProductVM { get; set; }
        public ProductController(IUnitOfWork UnitOfWork, IWebHostEnvironment hostEnvironment)
        {

            _UnitOfWork = UnitOfWork;
            _hostEnvironment = hostEnvironment;

            ProductVM = new ProductViewModel()
            {
                Product = new(),
                CategoryList = _UnitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _UnitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),


            };
        }

        //[Route("")]
        //[Route("~")]
        //[Route("Index")]
        public IActionResult Index()
        {
           
            return View();
        }

        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _UnitOfWork.Product.Add(obj);
        //        _UnitOfWork.Save();
        //        TempData["success"] = "CoverType Created Successfully";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(obj);
        //}

        [HttpGet]
        public IActionResult Upsert(int? Id)
        {

            //ProductViewModel ProductVM = new()
            //{
            //    Product = new(),
            //    CategoryList = _UnitOfWork.Category.GetAll().Select(i => new SelectListItem
            //    {
            //        Text = i.Name,
            //        Value = i.Id.ToString()
            //    }),
            //    CoverTypeList = _UnitOfWork.CoverType.GetAll().Select(i => new SelectListItem
            //    {
            //        Text = i.Name,
            //        Value = i.Id.ToString()
            //    }),
            //};

            if (Id == null || Id == 0)
            {
               
                return View(ProductVM);
            }
            else
            {
                ProductVM.Product = _UnitOfWork.Product.GetFirstOrDefault(u => u.Id == Id);
                return View(ProductVM);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Product.ImageUrl != null)
                    {
                        var oldImage = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }
                if(obj.Product.Id == 0)
                {
                    _UnitOfWork.Product.Add(obj.Product);
                }
                else
                {
                    _UnitOfWork.Product.Update(obj.Product);
                }
               
                _UnitOfWork.Save();
                TempData["success"] = "Product Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        //public IActionResult Delete(int Id)
        //{
        //    //var data=_db.Categories.Find(Id);
        //    var categoryFromDbFirst = _UnitOfWork.Product.GetFirstOrDefault(u => u.Id == Id);
        //    if (Id == null || Id == 0)
        //    {
        //        return NotFound();
        //    }
        //    return View(categoryFromDbFirst);

        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]

        //public IActionResult DeletePost(int? Id)
        //{
        //    var data = _UnitOfWork.Product.GetFirstOrDefault(u => u.Id == Id);
        //    if (Id == null)
        //    {
        //        return View(data);
        //    }
        //    _UnitOfWork.Product.Remove(data);
        //    _UnitOfWork.Save();
        //    TempData["success"] = "Product Deleted Successfully";
        //    return RedirectToAction(nameof(Index));

        //}

        #region API CALLS
        [HttpGet]
       
        //[Route("GetAll")]
        public IActionResult GetAll()
        {
            var prodList = _UnitOfWork.Product.GetAll(IncludeProperties:"Category, CoverType");
            return Json(new { data = prodList });
        }

        [HttpDelete]
       
        public IActionResult Delete(int? id)
        {
            var obj = _UnitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _UnitOfWork.Product.Remove(obj);
            _UnitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion

    }

}
