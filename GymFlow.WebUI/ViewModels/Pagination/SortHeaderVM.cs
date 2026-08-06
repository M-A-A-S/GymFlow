using GymFlow.Domain.DTOs.Common;

namespace GymFlow.WebUI.ViewModels.Pagination
{
    public class SortHeaderVM
    {
        public string Column { get; set; } = "";
        public string Title { get; set; } = "";
        public BaseFilterDTO Filter { get; set; } = default!;
        public Dictionary<string, string> Routes { get; set; } = new();
    }
}
