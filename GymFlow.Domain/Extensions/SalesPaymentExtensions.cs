using GymFlow.Domain.DTOs.SalesPayment;
using GymFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class SalesPaymentExtensions
    {
        public static SalesPaymentDTO ToDTO(this SalesPayment Entity)
        {
            if (Entity == null)
            {
                return null;
            }

            return new SalesPaymentDTO
            {
                Id = Entity.Id,
                SalesInvoiceId = Entity.SalesInvoiceId,
                Amount = Entity.Amount,
                PaymentMethod = Entity.PaymentMethod,
                PaymentDate = Entity.PaymentDate,
                ReferenceNo = Entity.ReferenceNo,
                Notes = Entity.Notes,
                IsVoided = Entity.IsVoided,
                VoidDate = Entity.VoidDate,
                VoidReason = Entity.VoidReason,
                VoidedBy = Entity.VoidedBy,
            };
        }

        public static SalesPayment ToEntity(this SalesPaymentDTO DTO)
        {
            if (DTO == null)
            {
                return null;
            }

            return new SalesPayment
            {
                Id = DTO.Id,
                SalesInvoiceId = DTO.SalesInvoiceId,
                Amount = DTO.Amount,
                PaymentMethod = DTO.PaymentMethod,
                PaymentDate = DTO.PaymentDate,
                ReferenceNo = DTO.ReferenceNo,
                Notes = DTO.Notes,
                IsVoided = DTO.IsVoided,
                VoidDate = DTO.VoidDate,
                VoidReason = DTO.VoidReason,
                VoidedBy = DTO.VoidedBy,
            };
        }

    }
}
