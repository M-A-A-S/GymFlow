using GymFlow.Application.Services;
using GymFlow.Domain.DTOs.Product;
using GymFlow.Domain.Resources.Shared;
using GymFlow.WebUI.Extensions;
using GymFlow.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GymFlow.WebUI.Controllers
{
    public class ProductsController : BaseController
    {
        #region ========================= Fields & Properties =========================
        private readonly IProductService _service;
        #endregion

        #region ========================= Constructors =========================
        public ProductsController(
            IProductService productService,
            IStringLocalizer<SharedResource> localizer
            ) : base(localizer)
        {
            _service = productService;
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

            var product = await GetEntityOrNull(_service.GetByIdAsync(id));

            if (product is null)
            {
                return NotFound();
            }

            return View(product);
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
            var result = await _service.GetProductAddUpdateDTO();
            return View(result.Data.ToViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductAddUpdateVM VM)
        {
            if (InvalidModel())
            {
                var result = await _service.GetProductAddUpdateDTO();
                result.Data.Product = VM.Product.ToDTO();
                return View(result.Data.ToViewModel());
            }

            var addResult = await _service.AddAsync(VM.Product.ToDTO());

            if (!addResult.IsSuccess)
            {
                Error(addResult.Code);
                var result = await _service.GetProductAddUpdateDTO();
                result.Data.Product = VM.Product.ToDTO();
                return View(result.Data.ToViewModel());
            }

            Success(addResult.Code);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region ========================= Update =========================
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _service.GetProductAddUpdateDTO(id);

            if (!result.IsSuccess)
            {
                Error(result.Code);
                return NotFound();
            }

            return View(result.Data.ToViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductAddUpdateVM VM)
        {
            if (InvalidModel())
            {
                var result = await _service.GetProductAddUpdateDTO(VM.Product.Id);
                result.Data.Product = VM.Product.ToDTO();
                return View(result.Data.ToViewModel());
            }

            var updateResult = await _service.UpdateAsync(VM.Product.Id, VM.Product.ToDTO());
            if (!updateResult.IsSuccess)
            {
                Error(updateResult.Code);
                var result = await _service.GetProductAddUpdateDTO(VM.Product.Id);
                result.Data.Product = VM.Product.ToDTO();
                return View(result.Data.ToViewModel());
            }

            Success(updateResult.Code);
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region ========================= Delete =========================
        public async Task<IActionResult> Delete(int id)
        {
            var product = await GetEntityOrNull(_service.GetByIdAsync(id));

            if (product is null)
            {
                return NotFound();
            }

            return View(product);
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
