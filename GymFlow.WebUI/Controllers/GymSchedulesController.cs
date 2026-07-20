using GymFlow.Application.Services;
using GymFlow.Domain.DTOs.GymSchedule;
using GymFlow.Domain.Resources.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GymFlow.WebUI.Controllers
{
    public class GymSchedulesController : BaseController
    {
        #region ========================= Fields & Properties =========================
        private readonly IGymScheduleService _service;

        #endregion

        #region ========================= Constructors =========================
        public GymSchedulesController(
            IGymScheduleService gymScheduleService,
            IStringLocalizer<SharedResource> localizer
            ) : base(localizer)
        {
            _service = gymScheduleService;
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
            var gymSchedule = await GetEntityOrNull(_service.GetByIdAsync(id));

            if (gymSchedule is null)
            {
                return NotFound();
            }

            return View(gymSchedule);
        }
        #endregion

        #region ========================= Create =========================
        public async Task<IActionResult> Create()
        {
            return View(new GymScheduleDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GymScheduleDTO DTO)
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
            var gymSchedule = await GetEntityOrNull(_service.GetByIdAsync(id));

            if (gymSchedule is null)
            {
                return NotFound();
            }

            return View(gymSchedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GymScheduleDTO DTO)
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
            var gymSchedule = await GetEntityOrNull(_service.GetByIdAsync(id));

            if (gymSchedule is null)
            {
                return NotFound();
            }

            return View(gymSchedule);

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
