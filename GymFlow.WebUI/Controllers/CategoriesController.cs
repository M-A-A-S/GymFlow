using GymFlow.Application.Services;
using GymFlow.Domain.DTOs.Category;
using GymFlow.Domain.DTOs.File;
using GymFlow.Domain.Resources.Shared;
using GymFlow.WebUI.Extensions;
using GymFlow.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Reflection.Metadata;

namespace GymFlow.WebUI.Controllers
{
    public class CategoriesController : BaseController
    {

        #region ========================= Fields & Properties =========================
        private readonly ICategoryService _service;
        #endregion

        #region ========================= Constructors =========================
        public CategoriesController(
            ICategoryService categoryService,
            IStringLocalizer<SharedResource> localizer
            ) : base(localizer)
        {
            _service = categoryService;
        }
        #endregion

        #region ========================= Get =========================
        public async Task<IActionResult> Index()
        {
            var getAllResult = await _service.GetAllAsync();
            return View(getAllResult.Data);
        }

        public async Task<IActionResult> Details(int id)
        {

            var category = await GetEntityOrNull(_service.GetByIdAsync(id));

            if (category is null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string search)
        {
            var result = await _service.SearchAsync(search);
            return Json(result.Data);
        }
        #endregion

        #region ========================= Create =========================
        public async Task<IActionResult> Create()
        {
            return View(new CategoryVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryVM VM)
        {
            if (InvalidModel())
            {
                return View(VM);
            }

            var addResult = await _service.AddAsync(VM.ToDTO());

            if (!addResult.IsSuccess)
            {
                Error(addResult.Code);
                return View(VM);
            }

            Success(addResult.Code);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region ========================= Update =========================
        public async Task<IActionResult> Edit(int id)
        {
            var category = await GetEntityOrNull(_service.GetByIdAsync(id));

            if (category is null)
            {
                return NotFound();
            }

            return View(category.ToViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryVM VM)
        {
            if (InvalidModel())
            {
                return View(VM);
            }

            var updateResult = await _service.UpdateAsync(VM.Id, VM.ToDTO());
            if (!updateResult.IsSuccess)
            {
                Error(updateResult.Code);
                return View(VM);
            }

            Success(updateResult.Code);
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region ========================= Delete =========================
        public async Task<IActionResult> Delete(int id)
        {
            var category = await GetEntityOrNull(_service.GetByIdAsync(id));

            if (category is null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var deleteResult = await _service.DeleteAsync(id);

            if (!deleteResult.IsSuccess)
            {
                Error(deleteResult.Code);
                return View(deleteResult.Data);
            }

            Success(deleteResult.Code);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region ========================= Helpers =========================
        

        #endregion
    }
}
