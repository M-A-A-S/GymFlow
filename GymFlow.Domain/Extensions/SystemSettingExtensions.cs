using GymFlow.Domain.DTOs.SystemSetting;
using GymFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class SystemSettingExtensions
    {
        public static SystemSettingDTO ToDTO(this SystemSetting Entity)
        {
            if (Entity == null)
            {
                return null;
            }

            return new SystemSettingDTO
            {
                Id = Entity.Id,
                NameEn = Entity.NameEn,
                NameAr = Entity.NameAr,
                AddressEn = Entity.AddressEn,
                AddressAr = Entity.AddressAr,
                Phone = Entity.Phone,
                Email = Entity.Email,
                Website = Entity.Website,
                Facebook = Entity.Facebook,
                Instagram = Entity.Instagram,
                Currency = Entity.Currency,
                ReceiptFooterEn = Entity.ReceiptFooterEn,
                ReceiptFooterAr = Entity.ReceiptFooterAr,
                TaxNumber = Entity.TaxNumber,
                LogoUrl = Entity.LogoUrl,
            };
        }

        public static SystemSetting ToEntity(this SystemSettingDTO DTO)
        {
            if (DTO == null)
            {
                return null;
            }

            return new SystemSetting
            {
                Id = DTO.Id,
                NameEn = DTO.NameEn,
                NameAr = DTO.NameAr,
                AddressEn = DTO.AddressEn,
                AddressAr = DTO.AddressAr,
                Phone = DTO.Phone,
                Email = DTO.Email,
                Website = DTO.Website,
                Facebook = DTO.Facebook,
                Instagram = DTO.Instagram,
                Currency = DTO.Currency,
                ReceiptFooterEn = DTO.ReceiptFooterEn,
                ReceiptFooterAr = DTO.ReceiptFooterAr,
                TaxNumber = DTO.TaxNumber,
                LogoUrl = DTO.LogoUrl,
            };
        }

    }
}
