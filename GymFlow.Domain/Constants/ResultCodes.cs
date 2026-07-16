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
    }
}
