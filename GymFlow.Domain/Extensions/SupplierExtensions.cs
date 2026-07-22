using GymFlow.Domain.DTOs.Supplier;
using GymFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Domain.Extensions
{
    public static class SupplierExtensions
    {
        public static SupplierDTO ToDTO(this Supplier Entity)
        {
            if (Entity == null)
            {
                return null;
            }

            return new SupplierDTO
            {
                Id = Entity.Id,
                FullName = Entity.FullName,
                PhoneNumber = Entity.PhoneNumber,
                Address = Entity.Address,
            };
        }

        public static Supplier ToEntity(this SupplierDTO DTO)
        {
            if (DTO == null)
            {
                return null;
            }

            return new Supplier
            {
                Id = DTO.Id,
                FullName = DTO.FullName,
                PhoneNumber = DTO.PhoneNumber,
                Address = DTO.Address,
            };
        }

    }
}
