using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.SystemSetting
{
    public class SystemSettingDTO
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
        public string? LogoUrl { get; set; }


        //public string WhatsApp { get; set; }

        //public string BranchName { get; set; }

        //public TimeOnly OpenTime { get; set; }

        //public TimeOnly CloseTime { get; set; }

        //public string CommercialRegistration { get; set; }

        //public string QRCodePath { get; set; } // Optional

    }
}
