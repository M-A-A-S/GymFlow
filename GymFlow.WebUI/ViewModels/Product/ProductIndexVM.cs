using GymFlow.Domain.DTOs.Category;
using GymFlow.Domain.DTOs.Product;
using GymFlow.Domain.Utilities;

namespace GymFlow.WebUI.ViewModels.Product
{
    public class ProductIndexVM
    {
        public PagedResult<ProductDTO> PagedResult { get; set; }
        public ProductFilterDTO Filter { get; set; }
        public IEnumerable<CategorySearchDTO> Categories { get; set; } = Enumerable.Empty<CategorySearchDTO>();
    }
}
