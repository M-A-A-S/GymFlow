using GymFlow.Domain.DTOs.PurchaseInvoice;
using GymFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class PurchaseInvoiceExtensions
    {
        public static PurchaseInvoiceDTO ToDTO(this PurchaseInvoice Entity)
        {
            if (Entity == null)
            {
                return null;
            }

            return new PurchaseInvoiceDTO
            {
                Id = Entity.Id,
                InvoiceNo = Entity.InvoiceNo,
                SupplierId = Entity.SupplierId,
                InvoiceDate = Entity.InvoiceDate,
                TotalAmount = Entity.TotalAmount,
                PaymentStatus = Entity.PaymentStatus,
                Notes = Entity.Notes,
            };
        }

        public static PurchaseInvoice ToEntity(this PurchaseInvoiceDTO DTO)
        {
            if (DTO == null)
            {
                return null;
            }

            return new PurchaseInvoice
            {
                Id = DTO.Id,
                InvoiceNo = DTO.InvoiceNo,
                SupplierId = DTO.SupplierId,
                InvoiceDate = DTO.InvoiceDate,
                TotalAmount = DTO.TotalAmount,
                PaymentStatus = DTO.PaymentStatus,
                Notes = DTO.Notes,
            };
        }

    }
}
