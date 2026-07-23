using GymFlow.Application.Services;
using GymFlow.Domain.DTOs.PurchaseInvoice;
using GymFlow.Domain.Resources.Shared;
using GymFlow.WebUI.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GymFlow.WebUI.Controllers
{
    public class PurchaseInvoicesController : BaseController
    {
        #region ========================= Fields & Properties =========================
        private readonly IPurchaseInvoiceService _service;
        #endregion

        #region ========================= Constructors =========================
        public PurchaseInvoicesController(
            IPurchaseInvoiceService purchaseInvoiceService,
            IStringLocalizer<SharedResource> localizer
            ) : base(localizer)
        {
            _service = purchaseInvoiceService;
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

            var purchaseInvoice = await GetEntityOrNull(_service.GetByIdAsync(id));

            if (purchaseInvoice is null)
            {
                return NotFound();
            }

            return View(purchaseInvoice);
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
            var result = await _service.GetPurchaseInvoiceAddUpdateDTO();
            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseInvoiceAddUpdateDTO DTO)
        {
            //foreach (var key in ModelState.Keys
            //.Where(x =>
            //    x.StartsWith("Supplier") ||
            //    x.StartsWith("PurchaseDetails") ||
            //    x.StartsWith("PurchasePayments"))
            //.ToList())
            //{
            //    ModelState.Remove(key);
            //}
            foreach (var key in ModelState.Keys
    .Where(x => x.StartsWith("PurchaseInvoice.Supplier"))
    .ToList())
            {
                ModelState.Remove(key);
            }
            if (InvalidModel())
            {
                var result = await _service.GetPurchaseInvoiceAddUpdateDTO();
                result.Data.PurchaseInvoice = DTO.PurchaseInvoice;
                result.Data.PurchaseInvoice.PurchaseDetails = DTO.PurchaseInvoice.PurchaseDetails;
                result.Data.PurchaseInvoice.PurchasePayments = DTO.PurchaseInvoice.PurchasePayments;
                return View(result.Data);
            }

            var addResult = await _service.AddAsync(DTO.PurchaseInvoice);

            if (!addResult.IsSuccess)
            {
                Error(addResult.Code);
                var result = await _service.GetPurchaseInvoiceAddUpdateDTO();
                result.Data.PurchaseInvoice = DTO.PurchaseInvoice;
                result.Data.PurchaseInvoice.PurchaseDetails = DTO.PurchaseInvoice.PurchaseDetails;
                result.Data.PurchaseInvoice.PurchasePayments = DTO.PurchaseInvoice.PurchasePayments;
                return View(result.Data);
            }

            Success(addResult.Code);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region ========================= Update =========================
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _service.GetPurchaseInvoiceAddUpdateDTO(id);

            if (!result.IsSuccess)
            {
                Error(result.Code);
                return NotFound();
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PurchaseInvoiceAddUpdateDTO DTO)
        {
            foreach (var key in ModelState.Keys
            .Where(x => x.StartsWith("PurchaseInvoice.Supplier"))
            .ToList())
            {
                ModelState.Remove(key);
            }
            if (InvalidModel())
            {
                var result = await _service.GetPurchaseInvoiceAddUpdateDTO(DTO.PurchaseInvoice.Id);
                result.Data.PurchaseInvoice = DTO.PurchaseInvoice;
                result.Data.PurchaseInvoice.PurchaseDetails = DTO.PurchaseInvoice.PurchaseDetails;
                result.Data.PurchaseInvoice.PurchasePayments = DTO.PurchaseInvoice.PurchasePayments;
                return View(result.Data);
            }

            var updateResult = await _service.UpdateAsync(DTO.PurchaseInvoice.Id, DTO.PurchaseInvoice);
            if (!updateResult.IsSuccess)
            {
                Error(updateResult.Code);
                var result = await _service.GetPurchaseInvoiceAddUpdateDTO(DTO.PurchaseInvoice.Id);
                result.Data.PurchaseInvoice = DTO.PurchaseInvoice;
                result.Data.PurchaseInvoice.PurchaseDetails = DTO.PurchaseInvoice.PurchaseDetails;
                result.Data.PurchaseInvoice.PurchasePayments = DTO.PurchaseInvoice.PurchasePayments;
                return View(result.Data);
            }

            Success(updateResult.Code);
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region ========================= Delete =========================
        public async Task<IActionResult> Delete(int id)
        {
            var purchaseInvoice = await GetEntityOrNull(_service.GetByIdAsync(id));

            if (purchaseInvoice is null)
            {
                return NotFound();
            }

            return View(purchaseInvoice);
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

    }
}
