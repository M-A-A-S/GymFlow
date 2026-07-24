using GymFlow.Domain.DTOs.SalesInvoice;
using GymFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class SalesInvoiceExtensions
    {
        public static SalesInvoiceDTO ToDTO(this SalesInvoice Entity)
        {
            if (Entity == null)
            {
                return null;
            }

            return new SalesInvoiceDTO
            {
                Id = Entity.Id,
                InvoiceNo = Entity.InvoiceNo,
                MemberId = Entity.MemberId,
                InvoiceDate = Entity.InvoiceDate,
                SubTotal = Entity.SubTotal,
                Discount = Entity.Discount,
                Tax = Entity.Tax,
                NetAmount = Entity.NetAmount,
                PaidAmount = Entity.PaidAmount,
                RemainingBalance = Entity.RemainingBalance,
                Status = Entity.Status,
                Notes = Entity.Notes,
            };
        }

        public static SalesInvoice ToEntity(this SalesInvoiceDTO DTO)
        {
            if (DTO == null)
            {
                return null;
            }

            return new SalesInvoice
            {
                Id = DTO.Id,
                InvoiceNo = DTO.InvoiceNo,
                MemberId = DTO.MemberId,
                InvoiceDate = DTO.InvoiceDate,
                SubTotal = DTO.SubTotal,
                Discount = DTO.Discount,
                Tax = DTO.Tax,
                NetAmount = DTO.NetAmount,
                PaidAmount = DTO.PaidAmount,
                RemainingBalance = DTO.RemainingBalance,
                Status = DTO.Status,
                Notes = DTO.Notes,
            };
        }

    }
}
