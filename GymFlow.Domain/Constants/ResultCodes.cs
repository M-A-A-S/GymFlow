using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Constants
{
    public static partial class ResultCodes
    {
        public const string ServerError = "ServerError";
        public const string NotFound = "NotFound";
        public const string InvalidData = "InvalidData";
        public const string EmailExists = "EmailExists";
        public const string PhoneExists = "PhoneExists";

        public const string CreatedSuccessfully = "CreatedSuccessfully";
        public const string UpdatedSuccessfully = "UpdatedSuccessfully";
        public const string DeletedSuccessfully = "DeletedSuccessfully";
        public const string AlreadyExists = "AlreadyExists";
        public const string ValidationError = "ValidationError";
        public const string UnexpectedError = "UnexpectedError";

        public const string InvalidDaysPerWeek = "InvalidDaysPerWeek";
        public const string InvalidDuration = "InvalidDuration";
        public const string InvalidPrice = "InvalidPrice";

        public const string RequiredField = "RequiredField";
        public const string InvalidValue = "InvalidValue";
        public const string ValueOutOfRange = "ValueOutOfRange";
        public const string ValueCannotBeNegative = "ValueCannotBeNegative";

        public const string SubscriptionOverlap = "SubscriptionOverlap";

        public const string MemberAlreadyCheckedIn = "MemberAlreadyCheckedIn";
        public const string CheckInSuccess = "CheckInSuccess";
        public const string AttendanceNotFound = "AttendanceNotFound";
        public const string AlreadyCheckedOut = "AlreadyCheckedOut";
        public const string CheckOutSuccess = "CheckOutSuccess";


        public const string GymScheduleOverlap = "GymScheduleOverlap";


        public const string ProductCodeAlreadyExists = "ProductCodeAlreadyExists";
        public const string NameEnAlreadyExists = "NameEnAlreadyExists";
        public const string NameArAlreadyExists = "NameArAlreadyExists";


        public const string SupplierNotFound = "SupplierNotFound";
        public const string ProductNotFound = "ProductNotFound";
        public const string PurchaseDetailsRequired = "PurchaseDetailsRequired";
        public const string InvalidQuantity = "InvalidQuantity";
        public const string InvalidUnitPrice = "InvalidUnitPrice";
        public const string InvalidPaymentAmount = "InvalidPaymentAmount";
        public const string InvalidPaymentDate = "InvalidPaymentDate";
        public const string DuplicateProductInInvoice = "DuplicateProductInInvoice";
        public const string PaymentExceedsInvoiceTotal = "PaymentExceedsInvoiceTotal";
        public const string InvoiceNumberRequired = "InvoiceNumberRequired";
        public const string InvoiceNumberExists = "InvoiceNumberExists";

        public const string SalesDetailsRequired = "SalesDetailsRequired";
        public const string DuplicateItemInInvoice = "DuplicateItemInInvoice";
        public const string InvalidSaleItemType = "InvalidSaleItemType";
        //public const string ProductNotFound = "ProductNotFound";
        public const string SubscriptionPlanNotFound = "SubscriptionPlanNotFound";
        public const string ServiceNotFound = "ServiceNotFound";
        public const string MemberNotFound = "MemberNotFound";
        public const string InvoiceNotFound = "InvoiceNotFound";
        public const string AlreadyCancelled = "AlreadyCancelled";
        public const string CannotEditPostedInvoice = "CannotEditPostedInvoice";
        public const string CannotPayDraftInvoice = "CannotPayDraftInvoice";
        public const string InvoiceAlreadyConfirmed = "InvoiceAlreadyConfirmed";

        public const string InsufficientStock = "InsufficientStock";
        public const string InvalidStockQuantity = "InvalidStockQuantity";
        public const string InvalidStockMovementType = "InvalidStockMovementType ";



    }
}
