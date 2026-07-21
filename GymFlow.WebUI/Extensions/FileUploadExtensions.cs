using GymFlow.Domain.DTOs.File;

namespace GymFlow.WebUI.Extensions
{
    public static class FileUploadExtensions
    {
        public static FileUploadRequest? ToFileUploadRequest(this IFormFile? file)
        {
            if (file is null)
            {
                return null;
            }

            return new FileUploadRequest
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                Length = file.Length,
                Content = file.OpenReadStream(),
            };

        }

        public static IFormFile ToFormFile(this FileUploadRequest? file)
        {
            if (file is null)
            {
                return null;
            }

            return new FormFile(file.Content, 0, file.Length, "file", file.FileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = file.ContentType,
            };

        }

    }
}
