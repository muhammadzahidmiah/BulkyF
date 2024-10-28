using BulkyWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order cannot exactly match the Name.");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }
            return View();

        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb1 = _unitOfWork.Category.Get(x => x.Id == id);
            /* Category? categoryFromDb2 = _db.Categories.FirstOrDefault(x=>x.Id == id);*/
            /* Category? categoryFromDb3 = _db.Categories.Where(x => x.Id == id).FirstOrDefault();*/
            if (categoryFromDb1 == null)
            {
                return NotFound();
            }
            return View(categoryFromDb1);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category Updated Successfully";

                return RedirectToAction("Index");
            }
            return View();

        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb1 = _unitOfWork.Category.Get(x => x.Id == id);
            //Category? categoryFromDb2 = _db.Categories.FirstOrDefault(x => x.Id == id);
            //Category? categoryFromDb3 = _db.Categories.Where(x => x.Id == id).FirstOrDefault();
            if (categoryFromDb1 == null)
            {
                return NotFound();
            }
            return View(categoryFromDb1);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(x => x.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted Successfully";

            return RedirectToAction("Index");

        }
    }
}
