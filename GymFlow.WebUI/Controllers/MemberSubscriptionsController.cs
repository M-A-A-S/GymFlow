using GymFlow.Application.Services;
using GymFlow.Domain.DTOs.MemberSubscription;
using GymFlow.Domain.Resources.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GymFlow.WebUI.Controllers
{
    public class MemberSubscriptionsController : BaseController
    {
        #region ========================= Fields & Properties =========================
        private readonly IMemberSubscriptionService _service;

        #endregion

        #region ========================= Constructors =========================
        public MemberSubscriptionsController(
            IMemberSubscriptionService memberService,
            IStringLocalizer<SharedResource> localizer
            ) : base(localizer)
        {
            _service = memberService;
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
            var result = await _service.GetMemberSubscriptionAddUpdateDTO();
            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberSubscriptionAddUpdateDTO DTO)
        {
            if (InvalidModel())
            {
                var result = await _service.GetMemberSubscriptionAddUpdateDTO();
                result.Data.MemberSubscription = DTO.MemberSubscription;
                return View(result.Data);
            }

            var addResult = await _service.AddAsync(DTO.MemberSubscription);

            if (!addResult.IsSuccess)
            {
                Error(addResult.Code);
                var result = await _service.GetMemberSubscriptionAddUpdateDTO();
                result.Data.MemberSubscription = DTO.MemberSubscription;
                return View(result.Data);
            }

            Success(addResult.Code);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region ========================= Update =========================
        public async Task<IActionResult> Edit(int id)
        {

            var result = await _service.GetMemberSubscriptionAddUpdateDTO(id);

            if (!result.IsSuccess)
            {
                Error(result.Code);
                return NotFound();
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MemberSubscriptionAddUpdateDTO DTO)
        {
            if (InvalidModel())
            {
                var result = 
                    await _service.GetMemberSubscriptionAddUpdateDTO(
                        DTO.MemberSubscription.Id);

                result.Data.MemberSubscription = DTO.MemberSubscription;
                return View(result.Data);
            }

            var updateResult = 
                await _service.UpdateAsync(
                    DTO.MemberSubscription.Id, 
                    DTO.MemberSubscription);

            if (!updateResult.IsSuccess)
            {
                Error(updateResult.Code);
                var result =
                    await _service.GetMemberSubscriptionAddUpdateDTO(
                        DTO.MemberSubscription.Id);

                result.Data.MemberSubscription = DTO.MemberSubscription;
                return View(result.Data);
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
