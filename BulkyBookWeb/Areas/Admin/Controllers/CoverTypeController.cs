
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public CoverTypeController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> data = _UnitOfWork.CoverType.GetAll();
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _UnitOfWork.CoverType.Add(obj);
                _UnitOfWork.Save();
                TempData["success"] = "CoverType Created Successfully";
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
            var data = _UnitOfWork.CoverType.GetFirstOrDefault(u => u.Id == Id);
            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _UnitOfWork.CoverType.Update(obj);
                _UnitOfWork.Save();
                TempData["success"] = "CoverType Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public IActionResult Delete(int Id)
        {
            //var data=_db.Categories.Find(Id);
            var categoryFromDbFirst = _UnitOfWork.CoverType.GetFirstOrDefault(u => u.Id == Id);
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
            var data = _UnitOfWork.CoverType.GetFirstOrDefault(u => u.Id == Id);
            if (Id == null)
            {
                return View(data);
            }
            _UnitOfWork.CoverType.Remove(data);
            _UnitOfWork.Save();
            TempData["success"] = "CoverType Deleted Successfully";
            return RedirectToAction(nameof(Index));

        }

    }
}
