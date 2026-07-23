using GymFlow.Domain.DTOs.PurchaseDetail;
using GymFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class PurchaseDetailExtensions
    {
        public static PurchaseDetailDTO ToDTO(this PurchaseDetail Entity)
        {
            if (Entity == null)
            {
                return null;
            }

            return new PurchaseDetailDTO
            {
                Id = Entity.Id,
                PurchaseInvoiceId = Entity.PurchaseInvoiceId,
                ProductId = Entity.ProductId,
                Quantity = Entity.Quantity,
                UnitPrice = Entity.UnitPrice,
                Total = Entity.Total,

                Product = Entity?.Product?.ToDTO(),
            };
        }

        public static PurchaseDetail ToEntity(this PurchaseDetailDTO DTO)
        {
            if (DTO == null)
            {
                return null;
            }

            return new PurchaseDetail
            {
                Id = DTO.Id,
                PurchaseInvoiceId = DTO.PurchaseInvoiceId,
                ProductId = DTO.ProductId,
                Quantity = DTO.Quantity,
                UnitPrice = DTO.UnitPrice,
                //Total = DTO.Total,
                Total = DTO.Quantity * DTO.UnitPrice,
            };
        }

    }
}
