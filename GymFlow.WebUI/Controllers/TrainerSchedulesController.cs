using GymFlow.Application.Services;
using GymFlow.Domain.DTOs.TrainerSchedule;
using GymFlow.Domain.Resources.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GymFlow.WebUI.Controllers
{
    public class TrainerSchedulesController : BaseController
    {
        #region ========================= Fields & Properties =========================
        private readonly ITrainerScheduleService _service;

        #endregion

        #region ========================= Constructors =========================
        public TrainerSchedulesController(
            ITrainerScheduleService trainerScheduleService,
            IStringLocalizer<SharedResource> localizer
            ) : base(localizer)
        {
            _service = trainerScheduleService;
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
            var trainerSchedule = await GetEntityOrNull(_service.GetByIdAsync(id));

            if (trainerSchedule is null)
            {
                return NotFound();
            }

            return View(trainerSchedule);
        }
        #endregion

        #region ========================= Create =========================
        public async Task<IActionResult> Create()
        {
            var result = await _service.GetTrainerScheduleAddUpdateDTO();
            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainerScheduleAddUpdateDTO DTO)
        {
            if (InvalidModel())
            {
                var result = await _service.GetTrainerScheduleAddUpdateDTO();
                result.Data.TrainerSchedule = DTO.TrainerSchedule;
                return View(result.Data);
            }

            var addResult = await _service.AddAsync(DTO.TrainerSchedule);

            if (!addResult.IsSuccess)
            {
                Error(addResult.Code);
                var result = await _service.GetTrainerScheduleAddUpdateDTO();
                result.Data.TrainerSchedule = DTO.TrainerSchedule;
                return View(result.Data);
            }

            Success(addResult.Code);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region ========================= Update =========================
        public async Task<IActionResult> Edit(int id)
        {

            var result = await _service.GetTrainerScheduleAddUpdateDTO(id);

            if (!result.IsSuccess)
            {
                Error(result.Code);
                return NotFound();
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TrainerScheduleAddUpdateDTO DTO)
        {
            if (InvalidModel())
            {
                var result =
                    await _service.GetTrainerScheduleAddUpdateDTO(
                        DTO.TrainerSchedule.Id);

                result.Data.TrainerSchedule = DTO.TrainerSchedule;
                return View(result.Data);
            }

            var updateResult =
                await _service.UpdateAsync(
                    DTO.TrainerSchedule.Id,
                    DTO.TrainerSchedule);

            if (!updateResult.IsSuccess)
            {
                Error(updateResult.Code);
                var result =
                    await _service.GetTrainerScheduleAddUpdateDTO(
                        DTO.TrainerSchedule.Id);

                result.Data.TrainerSchedule = DTO.TrainerSchedule;
                return View(result.Data);
            }

            Success(updateResult.Code);
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region ========================= Delete =========================
        public async Task<IActionResult> Delete(int id)
        {
            var trainerSchedule = await GetEntityOrNull(_service.GetByIdAsync(id));

            if (trainerSchedule is null)
            {
                return NotFound();
            }

            return View(trainerSchedule);

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
