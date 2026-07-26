using GymFlow.Application.Services;
using GymFlow.Domain.DTOs.SalesInvoice;
using GymFlow.Domain.Resources.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GymFlow.WebUI.Controllers
{
    public class SalesInvoicesController : BaseController
    {

        #region ========================= Fields & Properties =========================
        private readonly ISalesInvoiceService _service;
        #endregion

        #region ========================= Constructors =========================
        public SalesInvoicesController(
            ISalesInvoiceService SalesInvoiceService,
            IStringLocalizer<SharedResource> localizer
            ) : base(localizer)
        {
            _service = SalesInvoiceService;
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

            var SalesInvoice = await GetEntityOrNull(_service.GetByIdAsync(id));

            if (SalesInvoice is null)
            {
                return NotFound();
            }

            return View(SalesInvoice);
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
            var result = await _service.GetSalesInvoiceAddUpdateDTO();
            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesInvoiceAddUpdateDTO DTO)
        {
            foreach (var key in ModelState.Keys
            .Where(x => x.StartsWith("SalesInvoice.Member"))
            .ToList())
            {
                ModelState.Remove(key);
            }
            if (InvalidModel())
            {
                var result = await _service.GetSalesInvoiceAddUpdateDTO();
                result.Data.SalesInvoice = DTO.SalesInvoice;
                result.Data.SalesInvoice.Details = DTO.SalesInvoice.Details;
                result.Data.SalesInvoice.Payments = DTO.SalesInvoice.Payments;
                return View(result.Data);
            }

            var addResult = await _service.AddAsync(DTO.SalesInvoice);

            if (!addResult.IsSuccess)
            {
                Error(addResult.Code);
                var result = await _service.GetSalesInvoiceAddUpdateDTO();
                result.Data.SalesInvoice = DTO.SalesInvoice;
                result.Data.SalesInvoice.Details = DTO.SalesInvoice.Details;
                result.Data.SalesInvoice.Payments = DTO.SalesInvoice.Payments;
                return View(result.Data);
            }

            Success(addResult.Code);
            //return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(Print), new { id = addResult.Data });
        }
        #endregion

        #region ========================= Print =========================
        [HttpGet]
        public async Task<IActionResult> Print(int id)
        {
            var salesInvoice = await GetEntityOrNull(_service.GetByIdAsync(id));

            if (salesInvoice is null)
            {
                return NotFound();
            }

            return View(salesInvoice);
        }

        #endregion

        #region ========================= Update =========================
        //public async Task<IActionResult> Edit(int id)
        //{
        //    var result = await _service.GetSalesInvoiceAddUpdateDTO(id);

        //    if (!result.IsSuccess)
        //    {
        //        Error(result.Code);
        //        return NotFound();
        //    }

        //    return View(result.Data);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(SalesInvoiceAddUpdateDTO DTO)
        //{
        //    foreach (var key in ModelState.Keys
        //    .Where(x => x.StartsWith("SalesInvoice.Member"))
        //    .ToList())
        //    {
        //        ModelState.Remove(key);
        //    }
        //    if (InvalidModel())
        //    {
        //        var result = await _service.GetSalesInvoiceAddUpdateDTO(DTO.SalesInvoice.Id);
        //        result.Data.SalesInvoice = DTO.SalesInvoice;
        //        result.Data.SalesInvoice.Details = DTO.SalesInvoice.Details;
        //        result.Data.SalesInvoice.Payments = DTO.SalesInvoice.Payments;
        //        return View(result.Data);
        //    }

        //    var updateResult = await _service.UpdateAsync(DTO.SalesInvoice.Id, DTO.SalesInvoice);
        //    if (!updateResult.IsSuccess)
        //    {
        //        Error(updateResult.Code);
        //        var result = await _service.GetSalesInvoiceAddUpdateDTO(DTO.SalesInvoice.Id);
        //        result.Data.SalesInvoice = DTO.SalesInvoice;
        //        result.Data.SalesInvoice.Details = DTO.SalesInvoice.Details;
        //        result.Data.SalesInvoice.Payments = DTO.SalesInvoice.Payments;
        //        return View(result.Data);
        //    }

        //    Success(updateResult.Code);
        //    return RedirectToAction(nameof(Index));
        //}

        #endregion

        #region ========================= Delete =========================
        public async Task<IActionResult> Delete(int id)
        {
            var SalesInvoice = await GetEntityOrNull(_service.GetByIdAsync(id));

            if (SalesInvoice is null)
            {
                return NotFound();
            }

            return View(SalesInvoice);
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
