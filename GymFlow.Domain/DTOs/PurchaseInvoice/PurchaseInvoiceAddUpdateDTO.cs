using GymFlow.Domain.DTOs.Product;
using GymFlow.Domain.DTOs.PurchaseDetail;
using GymFlow.Domain.DTOs.PurchasePayment;
using GymFlow.Domain.DTOs.Supplier;
using GymFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.PurchaseInvoice
{
    public class PurchaseInvoiceAddUpdateDTO
    {

        public PurchaseInvoiceAddUpdateDTO()
        {
            PurchaseInvoice = new PurchaseInvoiceDTO();

            PurchaseInvoice.PurchaseDetails = new List<PurchaseDetailDTO>
            {
                new PurchaseDetailDTO()
            };

            PurchaseInvoice.PurchasePayments = new List<PurchasePaymentDTO>
            {
                new PurchasePaymentDTO()
            };

            PurchaseInvoice.Supplier = new SupplierDTO();
        }

        public PurchaseInvoiceDTO PurchaseInvoice { get; set; } = null;

        public IList<SupplierSearchDTO> Suppliers { get; set; } = new List<SupplierSearchDTO>();
        public IList<ProductSearchDTO> Products { get; set; } = new List<ProductSearchDTO>();
    }
}
