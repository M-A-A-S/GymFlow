using GymFlow.Domain.DTOs.File;

namespace GymFlow.WebUI.ViewModels.SystemSetting
{
    public class SystemSettingVM
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string AddressEn { get; set; }
        public string AddressAr { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? TaxNumber { get; set; }
        public string Currency { get; set; } = "SDG";
        public string? ReceiptFooterEn { get; set; }
        public string? ReceiptFooterAr { get; set; }
        //public string LogoPath { get; set; }
        //public string? LogoUrl { get; set; }

        public ImageInputVM Image { get; set; } = new();

    }
}
