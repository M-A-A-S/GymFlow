using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Constants
{
    public static partial class ResultCodes
    {
        public const string FileNotFound = "FileNotFound";
        public const string FileSaved = "FileSaved";
        public const string FileDeleted = "FileDeleted";
        public const string FileInvalidExtension = "FileInvalidExtension";
        public const string FileTooLarge = "FileTooLarge";
        public const string FileSaveFailed = "FileSaveFailed";
        public const string FileDeleteFailed = "FileDeleteFailed";

    }
}
