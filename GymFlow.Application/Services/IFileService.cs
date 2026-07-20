using GymFlow.Domain.DTOs.File;
using GymFlow.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Application.Services
{
    public interface IFileService
    {
        Task<Result<string?>> SaveAsync(
        FileUploadRequest? file,
        string? imageUrl,
        string folder,
        CancellationToken cancellationToken = default);

        Task<Result<string?>> ReplaceAsync(
            FileUploadRequest? file,
            string? imageUrl,
            string? oldFileName,
            string folder,
            CancellationToken cancellationToken = default);

        Task<Result<bool>> DeleteAsync(
            string? fileName,
            string folder,
            CancellationToken cancellationToken = default);

    }
}
