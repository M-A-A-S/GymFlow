using GymFlow.Domain.DTOs.Supplier;
using GymFlow.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Application.Services
{
    public interface ISupplierService
    {
        Task<Result<int>> AddAsync(SupplierDTO dto);
        Task<Result<bool>> UpdateAsync(int id, SupplierDTO dto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<SupplierDTO>> GetByIdAsync(int id);
        Task<Result<IEnumerable<SupplierDTO>>> GetAllAsync();
        Task<Result<IEnumerable<SupplierSearchDTO>>> SearchAsync(string search);

    }
}
