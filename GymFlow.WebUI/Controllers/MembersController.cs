using GymFlow.Application.Services;
using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.Member;
using GymFlow.Domain.Resources.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GymFlow.WebUI.Controllers
{
    public class MembersController : BaseController
    {
        private readonly IMemberService _service;

        public MembersController(
            IMemberService memberService,
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
            //var findResult = await _service.GetByIdAsync(id);
            //if (findResult.Data == null || !findResult.IsSuccess)
            //{
            //    TempData["Error"] = _localizer[findResult.Code].Value;
            //    return NotFound();
            //}
            //return View(findResult.Data);

            var member = await GetEntityOrNull(_service.GetByIdAsync(id));

            if (member is null)
            {
                return NotFound();
            }

            return View(member);
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
            return View(new MemberDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberDTO DTO)
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
            //var findResult = await _service.GetByIdAsync(id);
            //if (findResult.Data == null || !findResult.IsSuccess)
            //{
            //    TempData["Error"] = _localizer[findResult.Code].Value;
            //    return NotFound();
            //}

            //return View(findResult.Data);

            var member = await GetEntityOrNull(_service.GetByIdAsync(id));

            if (member is null)
            {
                return NotFound();
            }

            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MemberDTO DTO)
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

            //var findResult = await _service.GetByIdAsync(id);
            //if (findResult.Data == null || !findResult.IsSuccess)
            //{
            //    TempData["Error"] = _localizer[findResult.Code].Value;
            //    return NotFound();
            //}
            //return View(findResult.Data);
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
