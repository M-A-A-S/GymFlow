using GymFlow.Domain.DTOs.Category;
using GymFlow.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Application.Services
{
    public interface ICategoryService
    {
        Task<Result<int>> AddAsync(CategoryDTO dto);
        Task<Result<bool>> UpdateAsync(int id, CategoryDTO dto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<CategoryDTO>> GetByIdAsync(int id);
        Task<Result<IEnumerable<CategoryDTO>>> GetAllAsync();
        Task<Result<IEnumerable<CategorySearchDTO>>> SearchAsync(string search);

    }
}
