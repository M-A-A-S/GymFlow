using GymFlow.Domain.DTOs.SalesInvoiceDetail;
using GymFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class SalesInvoiceDetailExtensions
    {
        public static SalesInvoiceDetailDTO ToDTO(this SalesInvoiceDetail Entity)
        {
            if (Entity == null)
            {
                return null;
            }

            return new SalesInvoiceDetailDTO
            {
                Id = Entity.Id,
                SalesInvoiceId = Entity.SalesInvoiceId,
                ItemType = Entity.ItemType,
                ItemId = Entity.ItemId,
                Description = Entity.Description,
                Quantity = Entity.Quantity,
                UnitPrice = Entity.UnitPrice,
                Discount = Entity.Discount,
                Total = Entity.Total,
            };
        }

        public static SalesInvoiceDetail ToEntity(this SalesInvoiceDetailDTO DTO)
        {
            if (DTO == null)
            {
                return null;
            }

            return new SalesInvoiceDetail
            {
                Id = DTO.Id,
                SalesInvoiceId = DTO.SalesInvoiceId,
                ItemType = DTO.ItemType,
                ItemId = DTO.ItemId,
                Description = DTO.Description,
                Quantity = DTO.Quantity,
                UnitPrice = DTO.UnitPrice,
                Discount = DTO.Discount,
                Total = DTO.Total,
            };
        }

    }
}
