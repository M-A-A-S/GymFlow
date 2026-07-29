using GymFlow.Domain.Constants;
using GymFlow.Domain.DTOs.SystemSetting;
using GymFlow.WebUI.ViewModels;
using GymFlow.WebUI.ViewModels.SystemSetting;

namespace GymFlow.WebUI.Extensions
{
    public static class SystemSettingExtensions
    {

        public static SystemSettingDTO ToDTO(this SystemSettingVM VM)
        {
            return new SystemSettingDTO
            {
                Id = VM.Id,
                NameEn = VM.NameEn,
                NameAr = VM.NameAr,
                AddressEn = VM.AddressEn,
                AddressAr = VM.AddressAr,
                Phone = VM.Phone,
                Email = VM.Email,
                Website = VM.Website,
                Facebook = VM.Facebook,
                Instagram = VM.Instagram,
                Currency = VM.Currency,
                ReceiptFooterEn = VM.ReceiptFooterEn,
                ReceiptFooterAr = VM.ReceiptFooterAr,
                TaxNumber = VM.TaxNumber,
                LogoUrl = VM.Image.Url,
                Image = VM.Image.File.ToFileUploadRequest(),
            };
        }

        public static SystemSettingVM ToViewModel(this SystemSettingDTO DTO)
        {
            return new SystemSettingVM
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

                Image = new ImageInputVM
                {
                    ExistingUrl = DTO.LogoUrl.GetImageUrl(Constants.SystemSettingsFolder),
                }
            };
        }

    }
}
