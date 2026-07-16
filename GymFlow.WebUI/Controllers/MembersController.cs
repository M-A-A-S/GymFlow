using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Member;
using GymFlow.Domain.Resources.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GymFlow.WebUI.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _service;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public MembersController(
            IMemberService memberService,
            IStringLocalizer<SharedResource> localizer
            )
        {
            _service = memberService;
            _localizer = localizer;
        }

        #region Get
        public async Task<IActionResult> Index()
        {
            var getAllResult = await _service.GetAllAsync();
            return View(getAllResult.Data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var findResult = await _service.GetByIdAsync(id);
            if (findResult.Data == null || !findResult.IsSuccess)
            {
                TempData["Error"] = _localizer[findResult.Code].Value;
                return NotFound();
            }
            return View(findResult.Data);
        }
        #endregion

        #region Create
        public async Task<IActionResult> Create()
        {
            return View(new MemberDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberDTO DTO)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = _localizer[ResultCodes.ValidationError].Value;
                return View(DTO);
            }

            var addResult = await _service.AddAsync(DTO);

            if (addResult.IsSuccess)
            {
                TempData["Success"] = _localizer[addResult.Code].Value;
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = _localizer[addResult.Code].Value;
            return View(DTO);
        }
        #endregion

        #region Update
        public async Task<IActionResult> Edit(int id)
        {
            var findResult = await _service.GetByIdAsync(id);
            if (findResult.Data == null || !findResult.IsSuccess)
            {
                TempData["Error"] = _localizer[findResult.Code].Value;
                return NotFound();
            }

            return View(findResult.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MemberDTO DTO)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = _localizer[ResultCodes.ValidationError].Value;
                return View(DTO);
            }

            var updateResult = await _service.UpdateAsync(DTO.Id, DTO);
            if (updateResult.IsSuccess)
            {
                TempData["Success"] = _localizer[updateResult.Code].Value;
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = _localizer[updateResult.Code].Value;
            return View(DTO);
        }

        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int id)
        {
            var findResult = await _service.GetByIdAsync(id);
            if (findResult.Data == null || !findResult.IsSuccess)
            {
                TempData["Error"] = _localizer[findResult.Code].Value;
                return NotFound();
            }
            return View(findResult.Data);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var deleteResult = await _service.DeleteAsync(id);

            if (deleteResult.IsSuccess)
            {
                TempData["Success"] = _localizer[deleteResult.Code].Value;
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = _localizer[deleteResult.Code].Value;
            return View(deleteResult.Data);
        }
        #endregion

    }
}
