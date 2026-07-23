using GymFlow.Domain.DTOs.PurchaseInvoice;
using GymFlow.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Application.Services
{
    public interface IPurchaseInvoiceService
    {
        Task<Result<int>> AddAsync(PurchaseInvoiceDTO dto);
        Task<Result<IEnumerable<PurchaseInvoiceDTO>>> GetAllAsync();
        Task<Result<PurchaseInvoiceDTO>> GetByIdAsync(int id);
        Task<Result<IEnumerable<PurchaseInvoiceSearchDTO>>> SearchAsync(string search);
        Task<Result<bool>> UpdateAsync(int id, PurchaseInvoiceDTO dto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<PurchaseInvoiceAddUpdateDTO>> GetPurchaseInvoiceAddUpdateDTO(int? id = null);

    }
}
