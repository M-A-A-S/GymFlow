using GymFlow.Domain.DTOs.PurchasePayment;
using GymFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class PurchasePaymentExtensions
    {
        public static PurchasePaymentDTO ToDTO(this PurchasePayment Entity)
        {
            if (Entity == null)
            {
                return null;
            }

            return new PurchasePaymentDTO
            {
                Id = Entity.Id,
                PurchaseInvoiceId = Entity.PurchaseInvoiceId,
                Amount = Entity.Amount,
                PaymentMethod = Entity.PaymentMethod,
                PaymentDate = Entity.PaymentDate,
                ReferenceNo = Entity.ReferenceNo,
                Notes = Entity.Notes,
            };
        }

        public static PurchasePayment ToEntity(this PurchasePaymentDTO DTO)
        {
            if (DTO == null)
            {
                return null;
            }

            return new PurchasePayment
            {
                Id = DTO.Id,
                PurchaseInvoiceId = DTO.PurchaseInvoiceId,
                Amount = DTO.Amount,
                PaymentMethod = DTO.PaymentMethod,
                PaymentDate = DTO.PaymentDate,
                ReferenceNo = DTO.ReferenceNo,
                Notes = DTO.Notes,
            };
        }

    }
}
