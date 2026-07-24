using GymFlow.Domain.DTOs.Category;
using GymFlow.Domain.DTOs.Member;
using GymFlow.Domain.DTOs.Product;
using GymFlow.Domain.DTOs.SubscriptionType;
using GymFlow.Domain.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.SalesInvoice
{
    public class SalesInvoiceAddUpdateDTO
    {
        public SalesInvoiceDTO SalesInvoice { get; set; } = null!;

        public IList<MemberSearchDTO> Members { get; set; } = new List<MemberSearchDTO>();
        public IList<ProductSearchDTO> Products { get; set; } = new List<ProductSearchDTO>();
        public IList<CategorySearchDTO> Categories { get; set; } = new List<CategorySearchDTO>();
        public IList<SubscriptionTypeSearchDTO> SubscriptionTypes { get; set; } = new List<SubscriptionTypeSearchDTO>();

    }
}
