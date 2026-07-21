using GymFlow.Domain.DTOs.Category;
using GymFlow.WebUI.ViewModels;

namespace GymFlow.WebUI.Extensions
{
    public static class CategoryExtensions
    {
        public static CategoryDTO ToDTO(this CategoryVM VM)
        {
            return new CategoryDTO
            {
                NameEn = VM.NameEn,
                NameAr = VM.NameAr,
                DescriptionEn = VM.DescriptionEn,
                DescriptionAr = VM.DescriptionAr,
                ImageUrl = VM.Image.Url,
                IsActive = VM.IsActive,
                Image = VM.Image.File.ToFileUploadRequest(),
            };
        }

        public static CategoryVM ToViewModel(this CategoryDTO DTO)
        {
            return new CategoryVM
            {
                NameEn = DTO.NameEn,
                NameAr = DTO.NameAr,
                DescriptionEn = DTO.DescriptionEn,
                DescriptionAr = DTO.DescriptionAr,
                Image = new ImageInputVM
                {
                    ExistingUrl = DTO.ImageUrl,
                }
            };
        }

    }
}
