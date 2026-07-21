using GymFlow.Domain.DTOs.MemberSubscription;
using GymFlow.Domain.DTOs.Product;
using GymFlow.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Application.Services
{
    public interface IProductService
    {
        Task<Result<int>> AddAsync(ProductDTO dto);
        Task<Result<bool>> UpdateAsync(int id, ProductDTO dto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<ProductDTO>> GetByIdAsync(int id);
        Task<Result<IEnumerable<ProductDTO>>> GetAllAsync();
        Task<Result<IEnumerable<ProductSearchDTO>>> SearchAsync(string search);
        Task<Result<ProductAddUpdateDTO>> GetProductAddUpdateDTO(int? id = null);

    }
}
