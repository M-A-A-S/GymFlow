using GymFlow.Domain.DTOs.Category;
using GymFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class CategoryExtensions
    {
        public static CategoryDTO ToDTO(this Category Entity)
        {
            if (Entity == null)
            {
                return null;
            }

            return new CategoryDTO
            {
                Id = Entity.Id,
                NameEn = Entity.NameEn,
                NameAr = Entity.NameAr,
                DescriptionEn = Entity.DescriptionEn,
                DescriptionAr = Entity.DescriptionAr,
                ImageUrl = Entity.ImageUrl,
                IsActive = Entity.IsActive,
            };
        }

        public static Category ToEntity(this CategoryDTO DTO)
        {
            if (DTO == null)
            {
                return null;
            }

            return new Category
            {
                Id = DTO.Id,
                NameEn = DTO.NameEn,
                NameAr = DTO.NameAr,
                DescriptionEn = DTO.DescriptionEn,
                DescriptionAr = DTO.DescriptionAr,
                ImageUrl = DTO.ImageUrl,
                IsActive = DTO.IsActive,
            };
        }

    }
}
