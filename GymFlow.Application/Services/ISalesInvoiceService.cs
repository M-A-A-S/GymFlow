using GymFlow.Domain.DTOs.SalesInvoice;
using GymFlow.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Application.Services
{
    public interface ISalesInvoiceService
    {
        Task<Result<int>> AddAsync(SalesInvoiceDTO dto);
        Task<Result<IEnumerable<SalesInvoiceDTO>>> GetAllAsync();
        Task<Result<SalesInvoiceDTO>> GetByIdAsync(int id);
        Task<Result<IEnumerable<SalesInvoiceSearchDTO>>> SearchAsync(string search);
        Task<Result<bool>> UpdateAsync(int id, SalesInvoiceDTO dto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<bool>> CancelAsync(int id, string? reason = null);
        Task<Result<SalesInvoiceAddUpdateDTO>> GetSalesInvoiceAddUpdateDTO(int? id = null);

    }
}
