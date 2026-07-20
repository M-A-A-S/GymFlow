using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.DTOs.File
{
    public class FileValidationOptions
    {
        public long MaxFileSizeBytes { get; set; } = 5 * 1024 * 1024; // 5 MB

        public string[] AllowedExtensions { get; set; } =
        {
        ".jpg",
        ".jpeg",
        ".png",
        ".gif",
        ".webp",
        ".svg"
    };
    }
}
