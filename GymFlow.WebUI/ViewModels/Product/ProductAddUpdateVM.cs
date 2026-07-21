using GymFlow.Domain.DTOs.Category;
using GymFlow.Domain.DTOs.Member;
using GymFlow.Domain.DTOs.MemberSubscription;
using GymFlow.WebUI.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.Product
{
    public class ProductAddUpdateVM
    {
        public ProductVM Product { get; set; } = new();

        public IEnumerable<CategorySearchDTO> Categories { get; set; } = Enumerable.Empty<CategorySearchDTO>();

    }
}
