using GymFlow.Domain.DTOs.Product;
using GymFlow.WebUI.ViewModels;
using GymFlow.WebUI.ViewModels.Product;

namespace GymFlow.WebUI.Extensions
{
    public static class ProductExtensions
    {
        public static ProductDTO ToDTO(this ProductVM VM)
        {
            return new ProductDTO
            {
                Code = VM.Code,
                NameEn = VM.NameEn,
                NameAr = VM.NameAr,
                DescriptionEn = VM.DescriptionEn,
                DescriptionAr = VM.DescriptionAr,
                CategoryId = VM.CategoryId,
                PurchasePrice = VM.PurchasePrice.Value,
                SalePrice = VM.SalePrice.Value,
                Quantity = VM.Quantity.Value,
                ReorderLevel = VM.ReorderLevel.Value,
                ImageUrl = VM.Image.Url,
                Image = VM.Image.File.ToFileUploadRequest(),
            };
        }

        public static ProductVM ToViewModel(this ProductDTO DTO)
        {
            return new ProductVM
            {
                Code = DTO.Code,
                NameEn = DTO.NameEn,
                NameAr = DTO.NameAr,
                DescriptionEn = DTO.DescriptionEn,
                DescriptionAr = DTO.DescriptionAr,
                CategoryId = DTO.CategoryId,
                PurchasePrice = DTO.PurchasePrice,
                SalePrice = DTO.SalePrice,
                Quantity = DTO.Quantity,
                ReorderLevel = DTO.ReorderLevel,

                Image = new ImageInputVM
                {
                    ExistingUrl = DTO.ImageUrl.GetImageUrl("Categories"),
                    Prefix = "Product.Image"
                }
            };
        }

        public static ProductAddUpdateVM ToViewModel(this ProductAddUpdateDTO DTO)
        {
            return new ProductAddUpdateVM
            {
                Product = DTO.Product.ToViewModel(),
                Categories = DTO.Categories
            };
        }

    }
}
