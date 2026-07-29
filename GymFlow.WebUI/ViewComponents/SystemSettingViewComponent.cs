using GymFlow.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymFlow.WebUI.ViewComponents
{
    public class SystemSettingViewComponent : ViewComponent
    {
        private readonly ISystemSettingService _service;

        public SystemSettingViewComponent(ISystemSettingService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _service.GetCurrentAsync();

            return View(result.IsSuccess ? result.Data : null);
        }

    }
}
