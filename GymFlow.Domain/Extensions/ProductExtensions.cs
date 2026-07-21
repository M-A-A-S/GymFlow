using GymFlow.Domain.DTOs.Product;
using GymFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class ProductExtensions
    {
        public static ProductDTO ToDTO(this Product Entity)
        {
            if (Entity == null)
            {
                return null;
            }

            return new ProductDTO
            {
                Id = Entity.Id,
                Code = Entity.Code,
                NameEn = Entity.NameEn,
                NameAr = Entity.NameAr,
                DescriptionEn = Entity.DescriptionEn,
                DescriptionAr = Entity.DescriptionAr,
                ImageUrl = Entity.ImageUrl,
                CategoryId = Entity.CategoryId,
                PurchasePrice = Entity.PurchasePrice,
                SalePrice = Entity.SalePrice,
                Quantity = Entity.Quantity,
                ReorderLevel = Entity.ReorderLevel,

                Category = Entity?.Category?.ToDTO()
            };
        }

        public static Product ToEntity(this ProductDTO DTO)
        {
            if (DTO == null)
            {
                return null;
            }

            return new Product
            {
                Id = DTO.Id,
                Code = DTO.Code,
                NameEn = DTO.NameEn,
                NameAr = DTO.NameAr,
                DescriptionEn = DTO.DescriptionEn,
                DescriptionAr = DTO.DescriptionAr,
                ImageUrl = DTO.ImageUrl,
                CategoryId = DTO.CategoryId,
                PurchasePrice = DTO.PurchasePrice.Value,
                SalePrice = DTO.SalePrice.Value,
                Quantity = DTO.Quantity.Value,
                ReorderLevel = DTO.ReorderLevel.Value,
            };
        }

    }
}
