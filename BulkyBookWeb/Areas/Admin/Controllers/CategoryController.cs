
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public CategoryController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> data = _UnitOfWork.Category.GetAll();
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _UnitOfWork.Category.Add(obj);
                _UnitOfWork.Save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var data = _UnitOfWork.Category.GetFirstOrDefault(u => u.Id == Id);
            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _UnitOfWork.Category.Update(obj);
                _UnitOfWork.Save();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public IActionResult Delete(int Id)
        {
            //var data=_db.Categories.Find(Id);
            var categoryFromDbFirst = _UnitOfWork.Category.GetFirstOrDefault(u => u.Id == Id);
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            return View(categoryFromDbFirst);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public IActionResult DeletePost(int? Id)
        {
            var data = _UnitOfWork.Category.GetFirstOrDefault(u => u.Id == Id);
            if (Id == null)
            {
                return View(data);
            }
            _UnitOfWork.Category.Remove(data);
            _UnitOfWork.Save();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction(nameof(Index));

        }

    }
}
