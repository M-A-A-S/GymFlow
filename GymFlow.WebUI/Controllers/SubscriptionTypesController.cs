using GymFlow.Application.Services;
using GymFlow.Domain.DTOs.SubscriptionType;
using GymFlow.Domain.Resources.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GymFlow.WebUI.Controllers
{
    public class SubscriptionTypesController : BaseController
    {
        private readonly ISubscriptionTypeService _service;

        public SubscriptionTypesController(
            ISubscriptionTypeService memberService,
            IStringLocalizer<SharedResource> localizer
            ) : base(localizer)
        {
            _service = memberService;
        }

        #region ========================= Get =========================
        public async Task<IActionResult> Index()
        {
            var getAllResult = await _service.GetAllAsync();
            return View(getAllResult.Data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var member = await GetEntityOrNull(_service.GetByIdAsync(id));

            if (member is null)
            {
                return NotFound();
            }

            return View(member);
        }
        #endregion

        #region ========================= Create =========================
        public async Task<IActionResult> Create()
        {
            return View(new SubscriptionTypeDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubscriptionTypeDTO DTO)
        {
            if (InvalidModel())
            {
                return View(DTO);
            }

            var addResult = await _service.AddAsync(DTO);

            if (!addResult.IsSuccess)
            {
                Error(addResult.Code);
                return View(DTO);
            }

            Success(addResult.Code);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region ========================= Update =========================
        public async Task<IActionResult> Edit(int id)
        {

            var member = await GetEntityOrNull(_service.GetByIdAsync(id));

            if (member is null)
            {
                return NotFound();
            }

            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubscriptionTypeDTO DTO)
        {
            if (InvalidModel())
            {
                return View(DTO);
            }

            var updateResult = await _service.UpdateAsync(DTO.Id, DTO);
            if (!updateResult.IsSuccess)
            {
                Error(updateResult.Code);
                return View(DTO);
            }

            Success(updateResult.Code);
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region ========================= Delete =========================
        public async Task<IActionResult> Delete(int id)
        {
            var member = await GetEntityOrNull(_service.GetByIdAsync(id));

            if (member is null)
            {
                return NotFound();
            }

            return View(member);

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
