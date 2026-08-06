using GymFlow.Domain.DTOs.Category;
using GymFlow.Domain.Utilities;

namespace GymFlow.WebUI.ViewModels.Category
{
    public class CategoryIndexVM
    {
        public PagedResult<CategoryDTO> PagedResult { get; set; }
        public CategoryFilterDTO Filter { get; set; }
    }
}
