using GymFlow.Domain.Constants;
using GymFlow.Domain.Resources.Shared;
using GymFlow.Domain.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GymFlow.WebUI.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IStringLocalizer<SharedResource> _localizer;

        protected BaseController(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }


        protected void Success(string code)
        {
            TempData["Success"] = _localizer[code].Value;
        }

        protected void Error(string code)
        {
            TempData["Error"] = _localizer[code].Value;
        }

        protected bool InvalidModel()
        {
            if (ModelState.IsValid)
            {
                return false;
            }

            Error(ResultCodes.ValidationError);
            return true;
        }

        protected async Task<T?> GetEntityOrNull<T>(
            Task<Result<T>> task)
            where T : class
        {
            var result = await task;

            if (!result.IsSuccess || result.Data == null)
            {
                Error(result.Code);
                return null;
            }

            return result.Data;
        }

    }
}
